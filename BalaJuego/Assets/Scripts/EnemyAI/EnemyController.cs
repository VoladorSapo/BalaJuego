using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    StateMachine stateMachine;

    CharacterLife charater;

   public playerDetector detector { get; private set; }

    [field:SerializeField]   public Animator anim {get;private set;}

   [field:SerializeField] public float shootCadence { get; private set; }

    public float timeMagnitude { get; private set; }

    Vector3 initialPos;

public LevelAreaController area { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
       ServiceLocator.Instance.Get<ITimeManager>().subscribeToTimeChange(changeTimeMagnitude);
        timeMagnitude = 1;
        stateMachine = new StateMachine();

        detector = GetComponentInChildren<playerDetector>();
        ShootState shoot = new ShootState(this);
        IdleState idle = new IdleState(this);
        stateMachine.AddTransition(idle, shoot, new FuncPredicate(() => detector.reachableObjects.Count > 0));
        stateMachine.AddTransition(shoot, idle, new FuncPredicate(() => detector.reachableObjects.Count == 0));
        stateMachine.SetState(idle);
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
    private void Awake()
    {
        initialPos = transform.position;

    }
    void changeTimeMagnitude(object sender, timeData data)
    {
        timeMagnitude = data.currentMagnitude;
    }

    public void restart(LevelAreaController _area)
    {
        transform.position = initialPos;
        timeMagnitude = 1;
        area = _area;
    }
}
