using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gizmo : MonoBehaviour
{
    [SerializeField] Color color;
    [SerializeField] float sphereSize;
    [SerializeField] bool useScale;
    [SerializeField] bool onSelected = true;
    private void OnDrawGizmos()
    {
        if (onSelected)
            return;
        draw();
    }
    private void OnDrawGizmosSelected()
    {
        if(!onSelected)
            return;
        draw();
    }
    void draw()
    {
        Gizmos.color = color;
        float size = useScale ? (transform.localScale.x / 2) : sphereSize;
        Gizmos.DrawSphere(transform.position, size);
    }
}
