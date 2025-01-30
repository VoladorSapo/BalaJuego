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

  protected  StateMachine stateMachine;

    CharacterLife charater;

    public playerDetector detector { get; private set; }

    [field:SerializeField]   public Animator anim {get;private set;}

   [field:SerializeField] public float shootCadence { get; private set; }

    public float timeMagnitude { get; private set; }

    Vector3 initialPos;

  [field:SerializeField]  public Collider2D stunedCollider { get; private set; }

    public LevelAreaController area { get; private set; }


    // Start is called before the first frame update
   protected virtual void Start()
    {
       ServiceLocator.Instance.Get<ITimeManager>().subscribeToTimeChange(changeTimeMagnitude);
        timeMagnitude = 1;

        detector = GetComponentInChildren<playerDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        IGameState.gameState state = ServiceLocator.Instance.Get<IGameState>().getState();
        if (state == IGameState.gameState.NormalTime || state == IGameState.gameState.SlowDown)
        {
            stateMachine.Update();
        }

    }
    private void FixedUpdate()
    {
       // print("fixedUpdate");
        IGameState.gameState state = ServiceLocator.Instance.Get<IGameState>().getState();
        if (state == IGameState.gameState.NormalTime || state == IGameState.gameState.SlowDown)
        {
           // print("yess");
            stateMachine.FixedUpdate();
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

    public virtual void restart(LevelAreaController _area)
    {
        stunedCollider.gameObject.SetActive(false);
        transform.position = initialPos;
        timeMagnitude = 1;
        area = _area;
       

    }
}
