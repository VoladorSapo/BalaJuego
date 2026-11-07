using System;
using System.Collections;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;

public class TimeLimited : MonoBehaviour
{
    protected float timeMagnitude;
    [SerializeField] bool Infinite;
    [SerializeField] float lifeTime;

    
    float leftLifeTime;
    bool timeRunning;

    DestroyRespawnController destroyRespawnController;
    private void Awake()
    {
        IProyectile proyectileMovement = GetComponent<IProyectile>();
        if (proyectileMovement != null)
        {
            proyectileMovement.setActivateEvent((CharacterInteractor interactor) => ActivateTime());
        }
        destroyRespawnController = GetComponent<DestroyRespawnController>();
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
                if (destroyRespawnController != null)
                {
                    destroyRespawnController.Destroy();
                }
                else
                {
                    Destroy(gameObject);
                }
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
