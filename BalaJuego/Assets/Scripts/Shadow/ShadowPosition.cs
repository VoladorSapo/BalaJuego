using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPosition : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private LayerMask groundLayer; 
    [SerializeField] private float maxRayDistance = 20f; 

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, maxRayDistance, groundLayer);

        if (hit.collider != null)
        {
            Shader.SetGlobalVector("_PlayerWorldPos", hit.point);
        }
        else
        {
            Shader.SetGlobalVector("_PlayerWorldPos", transform.position);
        }

        Shader.SetGlobalFloat("_PlayerFloorDistance", hit.distance);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * maxRayDistance));
    }
}
