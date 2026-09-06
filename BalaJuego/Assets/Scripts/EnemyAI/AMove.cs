using UnityEngine;

public abstract class AMove : MonoBehaviour
{
    protected Transform charTransform;
    [SerializeField] protected float speed;
    protected float timeMagnitude;

    public abstract void UpdateMovement();
    public abstract void StartMovement();


    protected virtual void Start()
    {
        charTransform = GetComponent<Transform>();
        ServiceLocator.Instance.Get<ITimeManager>().subscribeToTimeChange(changeTimeMagnitude);
    }

    private void changeTimeMagnitude(object sender, timeData data)
    {
        timeMagnitude = data.currentMagnitude;
    }
}
