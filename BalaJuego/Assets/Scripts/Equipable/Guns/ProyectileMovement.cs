using System;
using UnityEngine;
using UnityEngine.Events;

public class ProyectileMovement :MonoBehaviour, IProyectile
{
    [SerializeField] float z;
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
    }
    private void Update()
    {
        if (moving)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime * timeMagnitude);
            transform.position = new Vector3(transform.position.x, transform.position.y, z);
        }
    }
    public virtual void ActivateProyectileMovement(CharacterInteractor shooter, float angle)
    {
        print("ActivateEvent");
        activateEvent?.Invoke(shooter);
        isShot?.Invoke();
        musicManager.Instance.PlayDisparo();
        //   print(shooter.transform.localScale.x);
        angle *= shooter.transform.localScale.x;
        transform.eulerAngles = new Vector3(0, shooter.transform.localScale.x < 0 ? -180 : 0, angle);
        moving = true;
        if (anim)
        {
            anim.Play("fly");
        }
    }
    public virtual void ActivateProyectileMovement(CharacterInteractor shooter, float angle, Vector3 pos)
    {
        ActivateProyectileMovement(shooter, angle);
        transform.position = new Vector3(pos.x, pos.y, z);
    }
    public void changeTimeMagnitude(object sender, timeData data)
    {
        timeMagnitude = data.currentMagnitude;
    }

    public GameObject getObj() => gameObject;

    public void hitSomething(GameObject obj)
    {
        moving = false;
    }

    public void setActivateEvent(Action<CharacterInteractor> activateSource)
    {
        print("setActivateEvent");
        activateEvent += activateSource;
    }
}