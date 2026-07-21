using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimestopEffects : MonoBehaviour
{

    Coroutine warpCoroutine, fadeInCoroutine, fadeOutCoroutine;

    [Header("SHOCKWAVE")]
    [SerializeField] Material mat;
    private Renderer rend;
    private static int waveDistance = Shader.PropertyToID("_WaveDistance");
    private static int ringSpawn = Shader.PropertyToID("_RingSpawn");
    GameObject shockWaveRenderer;
    [SerializeField] float shockwaveTime;
    [SerializeField] private AnimationCurve warpCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [Header("LIGHT FADE")]
    Light2D backGroundLight;
    float defaultIntensity;
    [SerializeField] float fadeOutTime;
    [SerializeField] float fadeInTime;

    // Start is called before the first frame update
    void Start()
    {
        shockWaveRenderer = Camera.main.transform.Find("ShockwaveRenderer").gameObject;

        backGroundLight = Camera.main.gameObject.GetComponentsInChildren<Light2D>(true)[0];
        defaultIntensity = backGroundLight.intensity;
        ServiceLocator.Instance.Get<ITimeManager>().subscribeToTimeChange(CallWarp);
        ServiceLocator.Instance.Get<ITimeManager>().subscribeToTimeChange(CallFade);
    }

    private void CallFade(object sender, timeData e)
    {
        if (backGroundLight == null)
        {
            Debug.Log("No se encontr� la luz");
            return;
        }
        if  (e.currentMagnitude < 1)
        {
            CallFadeOut();
        }
        else
        {
            CallFadeIn();
        }
    }
    private void CallWarp(object sender, timeData e)
    {
       
        if (warpCoroutine != null)
        {
            mat.SetFloat(waveDistance, -.6f);
            StopAllCoroutines();
        }
        if (e.currentMagnitude < 1)
        {
            Debug.Log("PARAR TIEMPO");
            Vector3 posicionViewport = Camera.main.WorldToViewportPoint(transform.position);
            float screenX = posicionViewport.x;
            float screenY = posicionViewport.y;
            mat.SetVector(ringSpawn, new Vector2(screenX, screenY));
            warpCoroutine = StartCoroutine(WarpCoroutine(-.6f, 1f));
        }
        else
        {
            Debug.Log("REANUDAR TIEMPO");
            shockWaveRenderer.SetActive(false);
        }
    }

    private void CallFadeOut()
    {


        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }
        float targetIntensity = defaultIntensity * .4f;
        fadeInCoroutine = StartCoroutine(FadeLightIntensity(backGroundLight.intensity, targetIntensity, fadeOutTime));
    }

    private void CallFadeIn()
    {

        if (fadeInCoroutine != null)
        {
            StopCoroutine(fadeInCoroutine);
        }
        float targetIntensity = defaultIntensity;
        fadeInCoroutine = StartCoroutine(FadeLightIntensity(backGroundLight.intensity, targetIntensity, fadeInTime));
    }

    private IEnumerator FadeLightIntensity(float v1, float v2, float time)
    {
        Debug.Log("fade light");
        float elapsed = 0f;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float normalized = elapsed / time;

            backGroundLight.intensity = Mathf.Lerp(v1, v2, normalized);
            yield return null;
        }
    }
    private IEnumerator WarpCoroutine(float v1, float v2)
    {
        shockWaveRenderer.SetActive(true);
        Vector3 posicionViewport = Camera.main.WorldToViewportPoint(transform.position);
        float screenX = posicionViewport.x;
        float screenY = posicionViewport.y;
        mat.SetVector(ringSpawn, new Vector2(screenX, screenY));
        float elapsedTime = 0f;
        while (elapsedTime < shockwaveTime)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / shockwaveTime;
            float curveProgress = warpCurve.Evaluate(normalizedTime);
            float currentDistance = Mathf.Lerp(v1, v2, curveProgress);
            mat.SetFloat(waveDistance, currentDistance);

            yield return null;
        }
        mat.SetFloat(waveDistance, v2);
        shockWaveRenderer.SetActive(false);
    }
}
