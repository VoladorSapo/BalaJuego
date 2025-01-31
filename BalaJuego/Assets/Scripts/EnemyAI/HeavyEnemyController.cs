using UnityEngine;

public class HeavyEnemyController: EnemyController
{

    [SerializeField]public WallDetector wallDetect;
    public Vector3 direction;

  public  Rigidbody2D rb2d;
    [field: SerializeField] public float speed { get; private set; }

    public bool finishCharging;

    public override void restart(LevelAreaController _area)
    {
        finishCharging = false;
        print("heavyRestart");
        base.restart(_area);
        wallDetect.gameObject.SetActive(false);

        if (stateMachine == null)
        {
            stateMachine = new StateMachine();
            StartChargeState startCharge = new StartChargeState(this);
            IdleState idle = new IdleState(this);
            ChargeState charge = new ChargeState(this);
            StunedState stuned = new StunedState(this);
            stateMachine.AddTransition(idle, startCharge, new FuncPredicate(() => detector.reachableObjects.Count > 0));
            //stateMachine.AddTransition(startCharge, idle, new FuncPredicate(() => detector.reachableObjects.Count == 0));
            stateMachine.AddTransition(startCharge, charge, new FuncPredicate(() => finishCharging == true));
            stateMachine.AddTransition(charge, idle, new FuncPredicate(() => finishCharging == false));


            stateMachine.AddTransition(charge, stuned, new FuncPredicate(() => wallDetect.wall != null));
        }
        stateMachine.SetState(new IdleState(this));
    }
}
