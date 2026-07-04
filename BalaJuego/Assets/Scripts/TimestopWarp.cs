using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimestopWarp : MonoBehaviour
{

    Coroutine warpCoroutine;

    [SerializeField] Material mat;
    private Renderer rend;
    private static int waveDistance = Shader.PropertyToID("_WaveDistance");
    private static int ringSpawn = Shader.PropertyToID("_RingSpawn");

    [SerializeField] float warpTime;
    [SerializeField] private AnimationCurve warpCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    // Start is called before the first frame update
    void Start()
    {

        ServiceLocator.Instance.Get<ITimeManager>().subscribeToTimeChange(CallWarp);
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
            Vector3 posicionViewport = Camera.main.WorldToViewportPoint(transform.position);
            float screenX = posicionViewport.x;
            float screenY = posicionViewport.y;
            mat.SetVector(ringSpawn, new Vector2(screenX, screenY));
            warpCoroutine = StartCoroutine(WarpCoroutine(-.6f, 1f));
        }
    }

    private IEnumerator WarpCoroutine(float v1, float v2)
    {
        Vector3 posicionViewport = Camera.main.WorldToViewportPoint(transform.position);
        float screenX = posicionViewport.x;
        float screenY = posicionViewport.y;
        mat.SetVector(ringSpawn, new Vector2(screenX, screenY));
        float elapsedTime = 0f;
        while (elapsedTime < warpTime)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / warpTime;
            float curveProgress = warpCurve.Evaluate(normalizedTime);
            float currentDistance = Mathf.Lerp(v1, v2, curveProgress);
            mat.SetFloat(waveDistance, currentDistance);

            yield return null;
        }
        mat.SetFloat(waveDistance, v2);
    }
}
