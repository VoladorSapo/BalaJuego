using UnityEngine;

public class HeavyEnemyBehaviour: AEnemyBehaviour
{

    public Vector3 direction;

  public  Rigidbody2D rb2d;
    [field: SerializeField] public float speed { get; private set; }
    [field: SerializeField] public float wallStunDuration { get; private set; }

    public bool finishCharging;


    public override void setUpStateMachine()
    {
        stateMachine = new StateMachine();
        StartChargeState startCharge = new StartChargeState(this);
        EnemyIdleState idle = new EnemyIdleState(this);
        EnemyHeavyChargeState charge = new EnemyHeavyChargeState(this);
        HeavyStunedState stuned = new HeavyStunedState(this);

        FuncPredicate detectPlayer = new FuncPredicate(() => detectorManager.detectorDictionary["in"].getCount() > 0);
        NotPredicate dontDetectPlayer = new NotPredicate(detectPlayer);
        FuncPredicate dontdetectground = new FuncPredicate(() => detectorManager.detectorDictionary["ground"].getCount() == 0);

        stateMachine.AddTransition(idle, startCharge, detectPlayer);

        stateMachine.AddTransition(charge, startCharge, new AndPredicate( dontdetectground, detectPlayer));
        stateMachine.AddTransition(charge, idle, new AndPredicate(dontdetectground, dontDetectPlayer));

        //stateMachine.AddTransition(startCharge, idle, new FuncPredicate(() => detector.reachableObjects.Count == 0));
        stateMachine.AddEndTransition(startCharge, charge, new TruePredicate());
        // stateMachine.AddTransition(charge, idle, new FuncPredicate(() => finishCharging == false));


       // stateMachine.AddTransition(charge, idle, new FuncPredicate(() => wallDetect.wall != null));

        stateMachine.AddEndTransition(stuned, idle, dontDetectPlayer);
        stateMachine.AddEndTransition(stuned,idle,detectPlayer);
        stateMachine.setDefaultState(idle);
        stateMachine.AddTransition(idle, startCharge, detectPlayer);

        detectorManager.detectorDictionary["wall"].addDetectEvent((object o) => { print("HEAVYSTUN WITH"); print(o == null); life.addEffect(new StunEffect(false, wallStunDuration)); });
    }
    
    public override void restart(LevelAreaController _area)
    {
        finishCharging = false;
        print("heavyRestart");
        base.restart(_area);
        detectorManager.detectorDictionary["wall"].gameObject.SetActive(false);

        
    }
    public override void startStunState()
    {
        stateMachine.SetState(new HeavyStunedState(null));
    }
    public override void endStunState()
    {
        stateMachine.ForceEndState(new HeavyStunedState(null));
    }

}
