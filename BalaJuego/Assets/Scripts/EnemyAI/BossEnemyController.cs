using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BossEnemyController : EnemyBehaviour
{
    IGun Charshoot;
    [SerializeField] GameObject gun;

    protected override void Start()
    {
        base.Start();
        Charshoot = GetComponentInChildren<IGun>();
    }

    public override void restart(LevelAreaController _area)
    {
        print("GunRestart");
        base.restart(_area);
        gun.SetActive(true);
        GetComponentInChildren<BossGun>().restart();
        if (stateMachine == null)
        {
            stateMachine = new StateMachine();
            EnemyShootState shoot = new EnemyShootState(this);
            EnemyIdleState idle = new EnemyIdleState(this);
            EnemyStunedState stuned = new EnemyStunedState(this);
            EnemyReloadState reload = new EnemyReloadState(this);
            stateMachine.AddTransition(idle, shoot, new FuncPredicate(() => detector.reachableObjects.Count > 0));
            stateMachine.AddTransition(shoot, idle, new FuncPredicate(() => detector.reachableObjects.Count == 0));
            stateMachine.AddAnyTransition(reload, new FuncPredicate(() => Charshoot.getBullets() == 0));
            stateMachine.AddTransition(reload, idle, new FuncPredicate(() => Charshoot.getBullets() > 0));
        }

        stateMachine.SetState(new EnemyIdleState(this));
        transform.position = initialPos;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(waitrestart());

    }
    IEnumerator waitrestart()
    {
        yield return new WaitForSeconds(0.1f);
        transform.position = initialPos;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().gravityScale = 1;
    }
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.tag == "TurnStun")
        {
            foreach (var item in FindObjectsOfType<baseBullet>())
            {
                Destroy(item.gameObject);
            }
            gun.SetActive(false);
            stateMachine.ForceSetState(new EnemyStunedState(this));
        }
    }
}
