using UnityEngine;

public class HeavyEnemyController: EnemyBehaviour
{

    [SerializeField]public WallDetector wallDetect;
    public Vector3 direction;

  public  Rigidbody2D rb2d;
    [field: SerializeField] public float speed { get; private set; }
    [field: SerializeField] public float wallStunDuration { get; private set; }

    public bool finishCharging;


    public void setUpStateMachine()
    {
        stateMachine = new StateMachine();
        StartChargeState startCharge = new StartChargeState(this);
        EnemyIdleState idle = new EnemyIdleState(this);
        EnemyHeavyChargeState charge = new EnemyHeavyChargeState(this);
        EnemyStunedState stuned = new EnemyStunedState(this);

        FuncPredicate detectPlayer = new FuncPredicate(() => detector.reachableObjects.Count > 0);
        FuncPredicate dontDetectPlayer = new FuncPredicate(() => detector.reachableObjects.Count == 0);

        stateMachine.AddTransition(idle, startCharge, detectPlayer);
        //stateMachine.AddTransition(startCharge, idle, new FuncPredicate(() => detector.reachableObjects.Count == 0));
        stateMachine.AddTransition(startCharge, charge, new FuncPredicate(() => finishCharging == true));
        // stateMachine.AddTransition(charge, idle, new FuncPredicate(() => finishCharging == false));


       // stateMachine.AddTransition(charge, idle, new FuncPredicate(() => wallDetect.wall != null));

        stateMachine.AddEndTransition(stuned, idle, dontDetectPlayer);
        stateMachine.AddEndTransition(stuned,idle,detectPlayer);

        print(life);
       wallDetect.addWallDetectEvent(() => { print("HEAVYSTUN"); life.addEffect(new StunEffect(false, wallStunDuration)); });
    }
    
    public override void restart(LevelAreaController _area)
    {
        finishCharging = false;
        print("heavyRestart");
        base.restart(_area);
        wallDetect.gameObject.SetActive(false);

        if (stateMachine == null)
        {
            setUpStateMachine();
        }
        stateMachine.SetState(new EnemyIdleState(this));
    }


   
}
