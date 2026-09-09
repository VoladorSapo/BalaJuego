using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FireVelocity : MonoBehaviour
{
    [SerializeField] private float Xsensibility = .05f;
    [SerializeField] private float Ysensibility = .05f;
    [SerializeField] private float maxTilt = .5f;
    [SerializeField] private float recoverSpeed = 8f;

    [SerializeField] private Renderer fireRenderer;

    private Material fireMat;
    private Vector2 lastWorldPos;
    private Vector2 currentOffset;

    private void Awake()
    {
        fireRenderer = GetComponent<Renderer>();

        fireMat = fireRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 currentWorldPos = transform.position;
        Vector2 worldDistance = currentWorldPos - lastWorldPos;
        Vector2 worldVelocity = worldDistance / Mathf.Max(Time.deltaTime, 0.0000001f);

        lastWorldPos = currentWorldPos;

        Vector3 localVelocity = transform.InverseTransformDirection(worldVelocity);

        float targetX = -localVelocity.x * Xsensibility;
        float targetY = -localVelocity.y * Ysensibility;

        targetX = Mathf.Clamp(targetX, -maxTilt, maxTilt);
        targetY = Mathf.Clamp(targetY, -maxTilt, maxTilt);

        Vector2 targetOffset = new Vector2(targetX, targetY);
        currentOffset = Vector2.Lerp(currentOffset, targetOffset, Time.deltaTime * recoverSpeed);
        fireMat.SetVector("_VelocityOffset", currentOffset);
    }
}
