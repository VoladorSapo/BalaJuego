using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class musicManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] clipsMenus;
    [SerializeField] private AudioClip[] clipIglesia;
    [SerializeField] private AudioClip[] clipNivel;
    [SerializeField] private AudioClip[] clipBoss;
    [SerializeField] private AudioClip[] clipCredits;
    [Range(0, 1)] public float volMusic = 1.0f;
    [Range(0, 1)] public float volSounds = 1.0f;

    private readonly Dictionary<string, AudioClip[]> songs = new();
    private AudioSource audioSourceA;
    private AudioSource audioSourceB;
    private AudioSource backgroundSoundSource;
    private bool isPlayingA = true;

    public AudioClip[] currentClips { get; private set; }
    private int currentPhase = 0;
    private const float FadeDuration = 0.4f;
    private float fadeTimer = 0f;
    private bool isTransitioning = false;
    private bool isSongTransitioning = false;
    private bool isBackgroundTransitioning = false;

    private string queuedSongID;
    private int queuedPhase = -1;
    private float queuedStartTime = 0f;

    private readonly List<AudioSource> soundPool = new();
    private const int PoolSize = 10;

    private float lastVolMusic = -1f;
    private float lastVolSounds = -1f;

    private AudioReverbZone reverbZone;

    public static musicManager Instance;

    [SerializeField] float pitchMult;

    float pitchObjective;

    [SerializeField] float transitionSpeed;


    private float walkSoundOffset = 0.3f;
    private float heavyWalkSoundOffset = 0.3f;
    private bool isWalking = false;
    private bool isHeavyWalking = false;
    private Coroutine walkCycleCoroutine;
    private Coroutine heavyWalkCycleCoroutine;


    public void Start()
    {
        

        pitchObjective = audioSourceA.pitch;
    }


    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        audioSourceA = CreateAudioSource(true);
        audioSourceB = CreateAudioSource(true);
        backgroundSoundSource = CreateAudioSource(true);


        reverbZone = GetComponent<AudioReverbZone>();
        if (reverbZone == null)
        {
            Debug.LogWarning("No se encontr� una AudioReverbZone en el GameObject.");
        }


        songs["menu"] = clipsMenus;
        songs["nivel"] = clipNivel;
        songs["iglesia"] = clipIglesia;
        songs["boss"] = clipBoss;
        songs["credits"] = clipCredits;

        InitializeSoundPool();


        // Inicializar
        SetSong("menu");
        PlayBackgroundSound("amb_desierto");
    }


    private void OnValidate()
    {
        volMusic = Mathf.Clamp01(volMusic);
        volSounds = Mathf.Clamp01(volSounds);
        UpdateMusicVolume();
        UpdateSoundVolume();
    }

    private void Update()
    {
        
        if (pitchObjective != audioSourceA.pitch)
        {
            audioSourceA.pitch = Mathf.MoveTowards(audioSourceA.pitch, pitchObjective, Time.deltaTime);
            audioSourceB.pitch = audioSourceA.pitch;
        }
        //*/

        // Detectar cambios en los volumenes
        if (!Mathf.Approximately(volMusic, lastVolMusic))
        {
            lastVolMusic = volMusic;
            UpdateMusicVolume();
        }

        if (!Mathf.Approximately(volSounds, lastVolSounds))
        {
            lastVolSounds = volSounds;
            UpdateSoundVolume();
        }

        // Transiciones de musica
        if (isTransitioning || isSongTransitioning)
        {
            fadeTimer += Time.deltaTime;
            float fadeProgress = Mathf.Clamp01(fadeTimer / FadeDuration);

            AudioSource activeSource = isPlayingA ? audioSourceA : audioSourceB;
            AudioSource newSource = isPlayingA ? audioSourceB : audioSourceA;

            activeSource.volume = Mathf.Lerp(volMusic, 0, fadeProgress);
            newSource.volume = Mathf.Lerp(0, volMusic, fadeProgress);

            if (fadeProgress >= 1f)
            {
                activeSource.Stop();
                EndTransition();
            }
        }

        // Transiciones de background
        if (isBackgroundTransitioning)
        {
            fadeTimer += Time.deltaTime;
            float fadeProgress = Mathf.Clamp01(fadeTimer / FadeDuration);

            backgroundSoundSource.volume = Mathf.Lerp(0, volSounds, fadeProgress);

            if (fadeProgress >= 1f)
            {
                isBackgroundTransitioning = false;
            }
        }
    }

    private void UpdateMusicVolume()
    {
        if (audioSourceA != null) audioSourceA.volume = volMusic;
        if (audioSourceB != null) audioSourceB.volume = volMusic;
        //Debug.Log($"Volumen de m�sica actualizado: {volMusic}");
    }

    private void UpdateSoundVolume()
    {
        foreach (var source in soundPool)
        {
            if (!source.isPlaying) continue;
            source.volume = volSounds;
        }
        if (backgroundSoundSource != null)
        {
            backgroundSoundSource.volume = volSounds;
        }
        //Debug.Log($"Volumen de sonidos actualizado: {volSounds}");
    }

    private AudioSource CreateAudioSource(bool loop)
    {
        var source = gameObject.AddComponent<AudioSource>();
        source.loop = loop;
        source.volume = volMusic;
        return source;
    }

    private void InitializeSoundPool()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            var source = CreateAudioSource(false);
            source.playOnAwake = false;
            source.volume = volSounds;
            source.enabled = false;
            soundPool.Add(source);
        }
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (var source in soundPool)
        {
            if (!source.isPlaying)
            {
                source.enabled = true;
                source.volume = volSounds;
                return source;
            }
        }

        var newSource = CreateAudioSource(false);
        newSource.playOnAwake = false;
        soundPool.Add(newSource);
        return newSource;
    }

    private void EndTransition()
    {
        isTransitioning = false;
        isSongTransitioning = false;
        fadeTimer = 0f;
        isPlayingA = !isPlayingA;

        if (!string.IsNullOrEmpty(queuedSongID))
        {
            SetSong(queuedSongID);
            queuedSongID = null;
        }
        else if (queuedPhase >= 0)
        {
            StartTransition(queuedPhase, queuedStartTime);
            queuedPhase = -1;
        }
    }

    public void PlayBackgroundSound(string soundName)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Sonidos/{soundName}");

        if (clip == null)
        {
            Debug.LogError($"El sonido de fondo '{soundName}' no se encontr� en la carpeta Resources/Sonidos.");
            return;
        }

        if (backgroundSoundSource.isPlaying)
        {
            StartBackgroundTransition(clip);
        }
        else
        {
            backgroundSoundSource.clip = clip;
            backgroundSoundSource.loop = true;
            backgroundSoundSource.volume = 0f;
            backgroundSoundSource.Play();
            isBackgroundTransitioning = true;
            fadeTimer = 0f;
        }
    }

    private void StartBackgroundTransition(AudioClip newClip)
    {
        backgroundSoundSource.Stop();
        backgroundSoundSource.clip = newClip;
        backgroundSoundSource.loop = true;
        backgroundSoundSource.volume = 0f;
        backgroundSoundSource.Play();
        isBackgroundTransitioning = true;
        fadeTimer = 0f;
    }

    public void SetPhase(int phase)
    {
        if (isTransitioning || isSongTransitioning || currentClips == null || phase == currentPhase) return;

        if (phase >= 0 && phase < currentClips.Length)
        {
            float currentTime = (isPlayingA ? audioSourceA : audioSourceB).time;
            StartTransition(phase, currentTime);
        }
    }

    public void SetSong(string songID)
    {
        if (isSongTransitioning || isTransitioning)
        {
            queuedSongID = songID;
            return;
        }

        if (!songs.TryGetValue(songID, out var clips))
        {
            Debug.LogError($"La canci�n '{songID}' no existe");
            return;
        }

        currentClips = clips;
        currentPhase = 0;
        PreloadClips(currentClips);
        StartSongTransition(0);
    }

    private void PreloadClips(AudioClip[] clips)
    {
        foreach (var clip in clips)
        {
            if (clip.loadState != AudioDataLoadState.Loaded)
            {
                clip.LoadAudioData();
            }
        }
    }

    private void StartSongTransition(float startTime)
    {
        AudioSource newSource = isPlayingA ? audioSourceB : audioSourceA;

        if (currentClips == null || currentClips.Length == 0)
        {
            //Debug.LogError("No hay clips disponibles para la transici�n.");
            return;
        }

        newSource.clip = currentClips[0];
        startTime = Mathf.Clamp(startTime, 0, newSource.clip.length);
        newSource.time = startTime;
        newSource.volume = volMusic;
        newSource.Play();

        isSongTransitioning = true;
        fadeTimer = 0f;
    }

    private void StartTransition(int newPhase, float startTime)
    {
        AudioSource newSource = isPlayingA ? audioSourceB : audioSourceA;

        if (currentClips == null || newPhase < 0 || newPhase >= currentClips.Length)
        {
            Debug.LogError("No hay clips disponibles para la transici�n o el �ndice est� fuera de rango.");
            return;
        }

        newSource.clip = currentClips[newPhase];
        startTime = Mathf.Min(startTime, newSource.clip.length - 0.1f);
        newSource.time = startTime;
        newSource.volume = volMusic;
        newSource.Play();

        isTransitioning = true;
        fadeTimer = 0f;
        currentPhase = newPhase;
    }

    public void PlaySound(string soundName)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Sonidos/{soundName}");

        if (clip == null)
        {
            Debug.LogError($"El sonido '{soundName}' no se encontr� en la carpeta Resources/Sonidos.");
            return;
        }
        print($"Poniendo sonido {soundName}");
        AudioSource audioSource = GetAvailableAudioSource();
        audioSource.pitch = 1.0f;
        audioSource.clip = clip;
        audioSource.volume = volSounds;
        audioSource.Play();
    }

    public void PlaySoundPitch(string soundName)
    {
        PlaySoundPitch(soundName, 0.15f);
    }


    public void PlaySoundPitch(string soundName, float pitchVariation = 0.1f)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Sonidos/{soundName}");

        if (clip == null)
        {
            Debug.LogError($"El sonido '{soundName}' no se encontr� en la carpeta Resources/Sonidos.");
            return;
        }

        pitchVariation = Mathf.Clamp(pitchVariation, 0f, 1f);

        AudioSource audioSource = GetAvailableAudioSource();
        audioSource.clip = clip;

        float minPitch = 1f - pitchVariation;
        float maxPitch = 1f + pitchVariation;

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.volume = volSounds;
        audioSource.Play();
    }


    public void MuteSong()
    {
        if (isPlayingA && audioSourceA.isPlaying)
        {
            audioSourceA.Pause();
        }
        else if (!isPlayingA && audioSourceB.isPlaying)
        {
            audioSourceB.Pause();
        }
    }

    public void ResumeSong()
    {
        if (isPlayingA && audioSourceA.clip != null)
        {
            audioSourceA.UnPause();
        }
        else if (!isPlayingA && audioSourceB.clip != null)
        {
            audioSourceB.UnPause();
        }
    }

    public void MuteBackground()
    {
        if (backgroundSoundSource.isPlaying)
        {
            backgroundSoundSource.Pause();
        }
    }

    public void ResumeBackground()
    {
        if (backgroundSoundSource.clip != null)
        {
            backgroundSoundSource.UnPause();
        }
    }

    private bool IsBackgroundSoundPlaying(string soundName)
    {
        // Aseg�rate de que el AudioSource no sea nulo y est� reproduciendo algo
        if (backgroundSoundSource != null && backgroundSoundSource.isPlaying)
        {
            // Compara el nombre del clip actual con el sonido solicitado
            return backgroundSoundSource.clip != null && backgroundSoundSource.clip.name == soundName;
        }

        return false; // No est� sonando nada o el AudioSource no est� configurado
    }



    public void changeTimeMagnitude(float value)
    {
        print("change");
        if (value == 1)
        {
            pitchObjective = 1;
        }
        else
        {
            pitchObjective = pitchMult;
        }
    }


    public void PlayJump()
    {
        string[] jumpSounds = { "snd_jump1", "snd_jump2", "snd_jump3" };
        string selectedSound = jumpSounds[Random.Range(0, jumpSounds.Length)];
        PlaySoundPitch(selectedSound);
    }
    public void PlayDisparo()
    {
        string[] shotSounds = { "snd_disparo1", "snd_disparo2", "snd_disparo3", "snd_disparo4", "snd_disparo5", "snd_disparo6" };
        string selectedSound = shotSounds[Random.Range(0, shotSounds.Length)];
        PlaySoundPitch(selectedSound);
    }
    public void PlayWalk()
    {
        string[] walkSounds = { "snd_paso1", "snd_paso2", "snd_paso3", "snd_paso4", "snd_paso5", "snd_paso6" };
        string selectedSound = walkSounds[Random.Range(0, walkSounds.Length)];
        PlaySoundPitch(selectedSound);
    }
    public void PlayHeavyWalk()
    {
        string[] heavyWalkSounds = { "snd_pasopesado1", "snd_pasopesado2", "snd_pasopesado3", "snd_pasopesado4" };
        string selectedSound = heavyWalkSounds[Random.Range(0, heavyWalkSounds.Length)];
        PlaySoundPitch(selectedSound);
    }

    public void StartWalking()
    {
        if (!isWalking)
        {
            isWalking = true;
            walkCycleCoroutine = StartCoroutine(PlayWalkCycle());
        }
    }

    public void StopWalking()
    {
        if (isWalking)
        {
            isWalking = false;
            if (walkCycleCoroutine != null)
                StopCoroutine(walkCycleCoroutine);
        }
    }

    private IEnumerator PlayWalkCycle()
    {
        while (isWalking)
        {
            PlayWalk();
            //Debug.Log("PASO" + walkSoundOffset);
            yield return new WaitForSeconds(walkSoundOffset);
        }
    }
    
    
    public void StartHeavyWalking()
    {
        if (!isHeavyWalking)
        {
            isHeavyWalking = true;
            heavyWalkCycleCoroutine = StartCoroutine(PlayHeavyWalkCycle());
        }
    }

    public void StopHeavyWalking()
    {
        print("stopHeavyWalking");
        if (isHeavyWalking)
        {
            isHeavyWalking = false;
            if (heavyWalkCycleCoroutine != null)
                StopCoroutine(heavyWalkCycleCoroutine);
        }
    }

    private IEnumerator PlayHeavyWalkCycle()
    {
        while (isHeavyWalking)
        {
            PlayHeavyWalk();
            //Debug.Log("PASO" + walkSoundOffset);
            yield return new WaitForSeconds(heavyWalkSoundOffset);
        }
    }

    public void FadeOutCurrentSong()
    {
        StartCoroutine(FadeOutCurrentSongCoroutine(FadeDuration));
    }

    private IEnumerator FadeOutCurrentSongCoroutine(float duration)
    {
        AudioSource currentSource = isPlayingA ? audioSourceA : audioSourceB;

        float startVolume = currentSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            currentSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }
        
        currentSource.volume = 0f;
        currentSource.Stop();
    }

    public bool IsMusicPlaying()
    {
        return (audioSourceA != null && audioSourceA.isPlaying) ||
               (audioSourceB != null && audioSourceB.isPlaying);
    }



}
