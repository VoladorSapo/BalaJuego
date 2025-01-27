using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gizmo : MonoBehaviour
{
    [SerializeField] Color color;
    [SerializeField] float sphereSize;
    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, sphereSize);
    }
}
