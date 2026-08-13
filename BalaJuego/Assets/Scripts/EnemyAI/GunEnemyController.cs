using UnityEngine;
public class GunEnemyController : EnemyBehaviour
{
    IGun Charshoot;

    [SerializeField] bool ChangeMeleeCollider;

    protected override void Start()
    {
        base.Start();
        Charshoot = GetComponentInChildren<IGun>();
    }

    public override void restart(LevelAreaController _area)
    {
        print("GunRestart");
        base.restart(_area);

        GetComponentInChildren<BaseGun>().restart();
        if (stateMachine == null)
        {
            stateMachine = new StateMachine();
            EnemyShootState shoot = new EnemyShootState(this);
            EnemyIdleState idle = new EnemyIdleState(this);
            EnemyStunedState stuned = new EnemyStunedState(this);
            stateMachine.AddTransition(idle, shoot, new FuncPredicate(() => detector.reachableObjects.Count > 0));
            stateMachine.AddTransition(shoot, idle, new FuncPredicate(() => detector.reachableObjects.Count == 0));
            stateMachine.AddAnyTransition(stuned, new FuncPredicate(() => Charshoot.getBullets() == 0 && canBeKilledMelee));
        }
        stateMachine.SetState(new EnemyIdleState(this));
        if (ChangeMeleeCollider)
        {
            canBeKilledMelee = true;
        }
    }
    public override void playAnimation(string animNameEnd)
    {
        base.playAnimation(animNameEnd);
    }
}
