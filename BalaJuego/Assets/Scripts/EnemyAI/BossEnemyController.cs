using UnityEngine;
public class BossEnemyController : EnemyController
{
    IShoot Charshoot;
    [SerializeField] GameObject gun;

    protected override void Start()
    {
        base.Start();
        Charshoot = GetComponentInChildren<IShoot>();
    }

    public override void restart(LevelAreaController _area)
    {
        print("GunRestart");
        base.restart(_area);
       gun.SetActive(true);
        GetComponentInChildren<BossShoot>().restart();
        if (stateMachine == null)
        {
            stateMachine = new StateMachine();
            ShootState shoot = new ShootState(this);
            IdleState idle = new IdleState(this);
            StunedState stuned = new StunedState(this);
            ReloadState reload = new ReloadState(this);
            stateMachine.AddTransition(idle, shoot, new FuncPredicate(() => detector.reachableObjects.Count > 0));
            stateMachine.AddTransition(shoot, idle, new FuncPredicate(() => detector.reachableObjects.Count == 0));
            stateMachine.AddAnyTransition(reload, new FuncPredicate(() => Charshoot.getBullets() == 0));
            stateMachine.AddTransition(reload, idle, new FuncPredicate(() => Charshoot.getBullets() > 0));
        }

        stateMachine.SetState(new IdleState(this));
        transform.position = initialPos;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

    }
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if(collision.tag == "TurnStun")
        {
            foreach (var item in FindObjectsOfType<baseBullet>())
            {
                Destroy(item.gameObject);
            }
            gun.SetActive(false);
            stateMachine.ForceSetState(new StunedState(this));
        }
    }
}
