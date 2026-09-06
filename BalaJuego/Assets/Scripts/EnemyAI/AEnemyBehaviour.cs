using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AEnemyBehaviour : MonoBehaviour
{
    public IState currentState()
    {
        if (stateMachine == null)
            return null;

        return stateMachine.currentState();

    }

    public StateMachine stateMachine;

    ACharacterLife charater;

    public playerDetector detector { get; private set; }

    [field: SerializeField] public Animator anim { get; private set; }
    [SerializeField] public bool differentFirstShootCadence = false;
    [field: SerializeField] public float firstShootCadence { get; private set; }

    [field: SerializeField] public float shootCadence { get; private set; }
    [field: SerializeField] public float shootCadenceRandomRange { get; private set; }

    [field: SerializeField] public float timeMagnitude { get; private set; }

    protected Vector3 initialPos;

    [field: SerializeField] public Collider2D stunedCollider { get; private set; }

    [field: SerializeField] public LevelAreaController area { get; private set; }

    public bool canBeKilledMelee = true;


    public ACharacterLife life;

    [field: SerializeField] public bool DebugOn { get; private set; }

    [SerializeField] GameObject FMarker;

    public abstract void setUpStateMachine();


    // Start is called before the first frame update
    protected virtual void Start()
    {
        ServiceLocator.Instance.Get<ITimeManager>().subscribeToTimeChange(changeTimeMagnitude);
        timeMagnitude = 1;
        life = GetComponent<ACharacterLife>();
        detector = GetComponentInChildren<playerDetector>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        IGameState.gameState state = ServiceLocator.Instance.Get<IGameState>().getState();
        if ((state == IGameState.gameState.NormalTime || state == IGameState.gameState.SlowDown) && !life.dead)
        {
            stateMachine?.Update();
        }
    }
    private void FixedUpdate()
    {
        // print("fixedUpdate");
        IGameState.gameState state = ServiceLocator.Instance.Get<IGameState>().getState();
        if ((state == IGameState.gameState.NormalTime || state == IGameState.gameState.SlowDown) && !life.dead)
        {
            // print("yess");
            stateMachine?.FixedUpdate();
        }
    }
    private void Awake()
    {
        initialPos = transform.position;

    }
    void changeTimeMagnitude(object sender, timeData data)
    {
        timeMagnitude = data.currentMagnitude;
    }
    //public void getStuned(float stunTime)
    //{
    //    durationOfCurrentStun = stunTime;
    //    stateMachine.ForceSetState(new EnemyStunedState(this));
    //}
    //public void endStun()
    //{
    //    stateMachine.ForceSetState(new EnemyIdleState(this));

    //}
    public virtual void restart(LevelAreaController _area)
    {
        detector?.restart();
        setColor(false);
        stunedCollider.gameObject.SetActive(false);
        GetComponent<ACharacterLife>().restart();
        transform.position = initialPos;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        timeMagnitude = 1;
        area = _area;
        if (stateMachine == null)
        {
            setUpStateMachine();
        }
        stateMachine.restart();

    }
    public void setColor(bool on)
    {
        print("setColor" + on);
        FMarker.SetActive(on);

        if (!on)
        {
            foreach (var item in GetComponentsInChildren<SpriteRenderer>())
            {
                item.color = new Color(1, 1, 1, 1);
            }
        }
        else
        {
            foreach (var item in GetComponentsInChildren<SpriteRenderer>())
            {
                if (item.name != "F")
                    item.color = new Color32(179, 42, 42, 255);
            }
        }
    }

    public virtual void playAnimation(string animNameEnd)
    {
        anim.Play("enemy" + animNameEnd);
    }

    static readonly Dictionary<int, string> stateNames = new Dictionary<int, string>
    { 
    { Animator.StringToHash("enemyShoot"), "Shoot" },
    { Animator.StringToHash("enemyLoad"),  "Load"  },
    { Animator.StringToHash("enemySpot"), "Spot" },
    { Animator.StringToHash("enemyIdleGun"),  "IdleGun"  },
    { Animator.StringToHash("enemyIdle"), "Idle" },

    };
    public static string getAnimStateNameFromHash(int hash)
    {
        if (!stateNames.ContainsKey(hash))
        {
            return "NULL";
        }
        else
        {
           return stateNames[hash];
        }
    }

    public virtual void startStunState()
    {
        stateMachine.ForceSetState(new EnemyStunedState(null));
    }
    public virtual void endStunState()
    {
        stateMachine.ForceEndState(new EnemyStunedState(null));
    }
}
