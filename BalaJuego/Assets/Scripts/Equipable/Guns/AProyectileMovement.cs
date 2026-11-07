using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class AProyectileMovement :MonoBehaviour, IProyectile

{
    [SerializeField] protected float z;
    [field: SerializeField] public float speed { get; protected set; }


    [field: SerializeField] public bool moving { get; protected set; }
    Animator anim;
    protected float timeMagnitude;

    IEffectSource source;

    UnityEvent isShot;

    Action<CharacterInteractor> activateEvent;

    private void Awake()
    {
    }
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        ITimeManager time = ServiceLocator.Instance.Get<ITimeManager>();
        timeMagnitude = time.getMagnitude();
        time.subscribeToTimeChange(changeTimeMagnitude);
        source = GetComponent<IEffectSource>();
        bool defaultMoving = moving;
        DestroyRespawnController respawnController = GetComponent<DestroyRespawnController>();
        if (respawnController != null)
            respawnController.subscribeToRespawnAction(() => moving = defaultMoving);

    }
    protected void Update()
    {
        if (moving)
        {
            Move();
        }
    }
    #region ActivateProyectile
    /*There are three ways of activating the movement of a proyectile
    You can send an angle, a position or a Transform.
    The default proyectile converts position and transform to angle
    */
    protected virtual void ActivateProyectile(CharacterInteractor shooter, Vector3 startPos)
    {
        transform.position = new Vector3(startPos.x, startPos.y, z);
        print("ActivateEvent");
        activateEvent?.Invoke(shooter);
        isShot?.Invoke();
        musicManager.Instance.PlayDisparo();
        moving = true;
        if (anim)
        {
            anim.Play("fly");
        }
    }
  
    #endregion
    public void changeTimeMagnitude(object sender, timeData data)
    {
        timeMagnitude = data.currentMagnitude;
    }

    public GameObject getObj() => gameObject;

    public void hitSomething(GameObject obj)
    {
        DeactivateMovement();
    }

    public void setActivateEvent(Action<CharacterInteractor> activateSource)
    {
        print("setActivateEvent");
        activateEvent += activateSource;
    }
    protected abstract void Move();

    public void DeactivateMovement()
    {
        moving = false;
    }

    public abstract void ActivateProyectileMovement(CharacterInteractor shooter, Vector3 startPos, float angle);

    public abstract void ActivateProyectileMovement(CharacterInteractor shooter, Vector3 startPos, Vector3 pos);

    public abstract void ActivateProyectileMovement(CharacterInteractor shooter, Vector3 startPos, Transform transform);
}
