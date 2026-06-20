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
            ShootState shoot = new ShootState(this);
            IdleState idle = new IdleState(this);
            StunedState stuned = new StunedState(this);
            stateMachine.AddTransition(idle, shoot, new FuncPredicate(() => detector.reachableObjects.Count > 0));
            stateMachine.AddTransition(shoot, idle, new FuncPredicate(() => detector.reachableObjects.Count == 0));
            stateMachine.AddAnyTransition(stuned, new FuncPredicate(() => Charshoot.getBullets() == 0 && canBeKilledMelee));
        }
        stateMachine.SetState(new IdleState(this));
        if (ChangeMeleeCollider)
        {
            canBeKilledMelee = true;
        }
    }

}
