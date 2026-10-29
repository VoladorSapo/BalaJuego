using UnityEngine;

public abstract class AEnemyMove : MonoBehaviour
{
    protected Transform charTransform;
    [SerializeField] protected float speed;
    [SerializeField] bool defaultObjectiveUpdate;
    /// <summary>
    /// How often the objective position is updated
    /// </summary>
    [SerializeField] float objectiveUpdateTime;

    [Header("Debug")]
    protected Vector3 startPosition;
    [SerializeField] protected float timeMagnitude;
    [SerializeField] protected Transform currentObjective;
    [field:SerializeField] public Vector3 currentObjectivePos {  get; protected set; }
    [SerializeField] protected bool objetiveUpdate;
    [SerializeField] float currentObjectiveUpdateTime;
    public abstract void StartMovement();
   public virtual void UpdateMovement()
    {
        if (objetiveUpdate && currentObjective)
        {
            currentObjectiveUpdateTime -= Time.deltaTime;
            if (objectiveUpdateTime <= 0)
            {
                currentObjectiveUpdateTime = objectiveUpdateTime;
                updateObjective();
            }
        }
    }

    protected virtual void Start()
    {
        charTransform = GetComponent<Transform>();
        ServiceLocator.Instance.Get<ITimeManager>().subscribeToTimeChange(changeTimeMagnitude);
    }

    private void changeTimeMagnitude(object sender, timeData data)
    {
        timeMagnitude = data.currentMagnitude;
    }

    public void setObjective(Transform objective)
    {
        currentObjective = objective;
        objetiveUpdate = defaultObjectiveUpdate;
        updateObjective();
    }
    protected virtual void updateObjective()
    {
        currentObjectivePos = currentObjective.position;

    }

    public void setObjective(Vector3 objectivePos)
    {
        currentObjective = null;
        currentObjectivePos = objectivePos;
        objetiveUpdate = false;
    }

}