using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider; 
    private void Start()
    {
        if (musicManager.Instance == null)
        {
            Debug.LogError("No hay MusicManager");
            return;
        }


        musicSlider.value = musicManager.Instance.volMusic;
        soundSlider.value = musicManager.Instance.volSounds;

        musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        soundSlider.onValueChanged.AddListener(UpdateSoundVolume);
    }

    private void UpdateMusicVolume(float value)
    {
        Debug.Log($"Música volumen: {value}");
        if (musicManager.Instance != null)
        {
            musicManager.Instance.volMusic = value;
        }
    }

    private void UpdateSoundVolume(float value)
    {
        Debug.Log($"Sonido volumen: {value}");
        if (musicManager.Instance != null)
        {
            musicManager.Instance.volSounds = value;
        }
    }


    private void OnDestroy()
    {
        musicSlider.onValueChanged.RemoveListener(UpdateMusicVolume);
        soundSlider.onValueChanged.RemoveListener(UpdateSoundVolume);
    }
}
