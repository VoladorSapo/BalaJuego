using UnityEngine;
public class GunEnemyBehaviour : AEnemyBehaviour
{
    protected IGun Charshoot;

    [SerializeField] bool ChangeMeleeCollider;

    protected override void Start()
    {
        base.Start();
        Charshoot = GetComponentInChildren<IGun>();
    }
    public override void setUpStateMachine()
    {
        stateMachine = new StateMachine();
        EnemyShootState shoot = new EnemyShootState(this);
        EnemyIdleState idle = new EnemyIdleState(this);
        EnemyStunedState stuned = new EnemyStunedState(this);
        stateMachine.AddTransition(idle, shoot, new FuncPredicate(() => detectorManager.detectorDictionary["in"].getCount() > 0));
        stateMachine.AddTransition(shoot, idle, new FuncPredicate(() => detectorManager.detectorDictionary["out"].getCount() == 0));
        stateMachine.AddAnyTransition(stuned, new FuncPredicate(() => Charshoot.getBullets() == 0 && canBeKilledMelee), "Empty");
        stateMachine.AddEndTransition(stuned, idle, new FuncPredicate(() => detectorManager.detectorDictionary["out"].getCount() == 0));
        stateMachine.AddEndTransition(stuned, shoot, new FuncPredicate(() => detectorManager.detectorDictionary["in"].getCount() > 0));

        stateMachine.setDefaultState(idle);

    }
    public override void restart(LevelAreaController _area)
    {
        // print("GunRestart");
        base.restart(_area);

        GetComponentInChildren<BaseGun>().restart();
        if (ChangeMeleeCollider)
        {
            canBeKilledMelee = true;
        }
    }
}
public class KnifeEnemyBehaviour : AEnemyBehaviour
{
    public override void setUpStateMachine()
    {

        stateMachine = new StateMachine();
        EnemyPlayAnimationState attack = new EnemyPlayAnimationState(this, "Attack");
        EnemyFollowPlayerState follow = new EnemyFollowPlayerState(this);
        EnemyIdleState idle = new EnemyIdleState(this);
        EnemyStunedState stuned = new EnemyStunedState(this);
        stateMachine.AddTransition(idle, follow, new FuncPredicate(() => detectorManager.detectorDictionary["in"].getCount() > 0));
        stateMachine.AddTransition(follow, idle, new FuncPredicate(() => detectorManager.detectorDictionary["out"].getCount() == 0));
        stateMachine.AddEndTransition(stuned, idle, new FuncPredicate(() => detectorManager.detectorDictionary["out"].getCount() == 0));
        stateMachine.AddEndTransition(stuned, attack, new FuncPredicate(() => detectorManager.detectorDictionary["attack"].getCount() > 0));
        stateMachine.AddEndTransition(stuned, follow, new FuncPredicate(() => detectorManager.detectorDictionary["in"].getCount() > 0));
        stateMachine.AddTransition(follow, attack, new FuncPredicate(() => detectorManager.detectorDictionary["attack"].getCount() > 0));
        stateMachine.AddEndTransition(attack, follow, new FuncPredicate(() => detectorManager.detectorDictionary["attack"].getCount() == 0));


        stateMachine.setDefaultState(idle);
    }
}
public class TurretEnemyBehaviour : GunEnemyBehaviour
{
    public override void setUpStateMachine()
    {
        stateMachine = new StateMachine();
        EnemyShootState shoot = new EnemyShootState(this);
        EnemyIdleState idle = new EnemyIdleState(this);
        EnemyStunedState stuned = new EnemyStunedState(this);
        stateMachine.AddTransition(idle, shoot, new FuncPredicate(() => detectorManager.detectorDictionary["in"].getCount() != 0));
        stateMachine.AddTransition(shoot, idle, new FuncPredicate(() => detectorManager.detectorDictionary["in"].getCount() == 0));
        stateMachine.AddAnyTransition(stuned, new FuncPredicate(() => Charshoot.getBullets() == 0 && canBeKilledMelee));
        stateMachine.setDefaultState(idle);
    }
}