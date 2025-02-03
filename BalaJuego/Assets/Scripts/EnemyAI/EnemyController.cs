using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public IState currentState() {
        if (stateMachine == null)
            return null;

      return  stateMachine.currentState();

        }

  public  StateMachine stateMachine;

    CharacterLife charater;

    public playerDetector detector { get; private set; }

    [field:SerializeField]   public Animator anim {get;private set;}

   [field:SerializeField] public float shootCadence { get; private set; }
    [field: SerializeField] public float shootCadenceRandomRange { get; private set; }

    public float timeMagnitude { get; private set; }

  protected  Vector3 initialPos;

  [field:SerializeField]  public Collider2D stunedCollider { get; private set; }

   [field:SerializeField] public LevelAreaController area { get; private set; }

    public bool canBeKilledMelee = true;
    [SerializeField] public bool shootOnShight = false;


    public CharacterLife life;

    [SerializeField] GameObject FMarker;
    // Start is called before the first frame update
   protected virtual void Start()
    {
       ServiceLocator.Instance.Get<ITimeManager>().subscribeToTimeChange(changeTimeMagnitude);
        timeMagnitude = 1;
        life = GetComponent<CharacterLife>();
        detector = GetComponentInChildren<playerDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        IGameState.gameState state = ServiceLocator.Instance.Get<IGameState>().getState();
        if ((state == IGameState.gameState.NormalTime || state == IGameState.gameState.SlowDown )&& !life.dead)
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
    public void getStuned()
    {
        stateMachine.ForceSetState(new StunedState(this));
    }
    public virtual void restart(LevelAreaController _area)
    {
        detector?.restart();
        setColor(false);
        stunedCollider.gameObject.SetActive(false);
        GetComponent<CharacterLife>().restart();
        transform.position = initialPos;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        timeMagnitude = 1;
        area = _area;
       

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
}
