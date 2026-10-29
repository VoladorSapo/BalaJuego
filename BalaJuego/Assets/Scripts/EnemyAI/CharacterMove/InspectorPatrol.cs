using UnityEngine;

public class InspectorPatrol : APatrol
{
    [SerializeField] Vector3[] setPoints;
    protected override Vector3[] calculatePatrolPoints()
    {
        return setPoints;
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = color;
        if (Application.isPlaying)
        {
            foreach (Vector3 p in setPoints)
            {
                Gizmos.DrawSphere(startPosition + p, radius);
            }
        }
        else
        {
            foreach (Vector3 p in setPoints)
            {
                Gizmos.DrawSphere(transform.position + p, radius);
            }
        }
    }
}