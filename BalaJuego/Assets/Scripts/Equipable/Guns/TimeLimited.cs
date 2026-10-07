using UnityEngine;

public class TimeLimited : MonoBehaviour
{
    protected float timeMagnitude;
    [SerializeField] bool Infinite;
    [SerializeField] float lifeTime;

    
    float leftLifeTime;
    bool timeRunning;

    private void Awake()
    {
        IProyectile proyectileMovement = GetComponent<IProyectile>();
        if (proyectileMovement != null)
        {
            proyectileMovement.setActivateEvent((CharacterInteractor interactor) => ActivateTime());
        }
    }
    private void Start()
    {
        ITimeManager time = ServiceLocator.Instance.Get<ITimeManager>();
        time.subscribeToTimeChange(changeTimeMagnitude);
       
    }
    private void Update()
    {
        if (!Infinite && timeRunning)
        {
            leftLifeTime -= Time.deltaTime * timeMagnitude;
            if (lifeTime <= 0)
            {
                ServiceLocator.Instance.Get<IsoftLock>().checkAll();
                // print("destroy");
                Destroy(gameObject);
            }
        }

    }
    public void ActivateTime()
    {
       timeRunning = true;
        leftLifeTime = lifeTime;
    }


    public void changeTimeMagnitude(object sender, timeData data)
    {
        timeMagnitude = data.currentMagnitude;
    }
    private void OnDestroy()
    {
        ServiceLocator.Instance.Get<ITimeManager>().unSubscribeToTimeChange(changeTimeMagnitude);

    }
}
