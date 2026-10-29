using UnityEngine;

public abstract class APatrol : MonoBehaviour
{
    protected Transform charTransform;
    AEnemyMove move;
    protected Vector3 startPosition;

    int current;
    [SerializeField] float changePointThreshold;

    [Header("Gizmo")]
    [SerializeField] protected Color color;
    [SerializeField] protected float radius;

    Vector3[] points;


    private void Start()
    {
        charTransform = GetComponent<Transform>();
        startPosition = charTransform.position;
        move = GetComponent<AEnemyMove>();
        points = calculatePatrolPoints();
    }
    public virtual void StartPatrol()
    {
        current = -1;
        nextPoint();
    }
    public virtual void UpdatePatrol()
    {
        if (Vector3.SqrMagnitude(move.currentObjectivePos - charTransform.position) < changePointThreshold)
        {
            nextPoint();
        }
    }
    void nextPoint()
    {
        current = (current + 1) % points.Length;
        move.setObjective(startPosition + points[current]);
    }
    protected abstract Vector3[] calculatePatrolPoints();
    protected abstract void OnDrawGizmos();
}
