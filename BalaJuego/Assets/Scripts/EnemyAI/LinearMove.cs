using UnityEngine;

public class LinearMove : AMove
{
    [SerializeField]  Vector3[] points;
    int current;
    Vector3 currentObjective;
    Vector3 startPosition;
    [SerializeField] float changePointThreshold;

    [Header("Gizmo")]
    [SerializeField] Color color;
    [SerializeField] float radius;
    public override void UpdateMovement()
    {
        charTransform.position = Vector3.MoveTowards(charTransform.position, currentObjective, speed * Time.deltaTime * timeMagnitude);
        if (Vector3.SqrMagnitude(currentObjective - charTransform.position) < changePointThreshold)
        {
            nextPoint();
        }
    }
    void nextPoint()
    {
        current = (current + 1) % points.Length;
        currentObjective = startPosition + points[current];
    }
    public override void StartMovement()
    {
     
    }

    protected override void Start()
    {
        base.Start();
        startPosition = charTransform.position;
        current = -1;
        nextPoint();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        if(Application.isPlaying){
            foreach (Vector3 p in points)
            {
                Gizmos.DrawSphere(startPosition + p, radius);
            }
        }
        else
        {
            foreach (Vector3 p in points)
            {
                Gizmos.DrawSphere(transform.position + p, radius);
            }
        }
    }
}