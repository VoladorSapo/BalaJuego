using UnityEngine;

public class IdleState : BaseEnemyState
{
    public IdleState(EnemyController _enemy)
    {
        enemy = _enemy;
    }

    public override void OnEnter()
    { 
        Debug.Log("start Idle");
        enemy.anim.Play("enemyIdle");
        if (enemy.GetComponentInChildren<IShoot>() != null)
        {
            enemy.GetComponentInChildren<IShoot>().getAnim().Play("gunIdle");
        }
    }
    public override void Update()
    {
    }
}

public class ShootState : BaseEnemyState
{

    float cadenceTime;
    public ShootState(EnemyController _enemy)
    {
        enemy = _enemy;
    }

    public override void OnEnter()
    {
        enemy.GetComponentInChildren<IShoot>().setShooting(false);
        enemy.anim.Play("enemySpot");
        enemy.GetComponentInChildren<IShoot>().getAnim().Play("enemyGunSpot");
        cadenceTime = enemy.shootCadence + Random.Range(-enemy.shootCadenceRandomRange, enemy.shootCadenceRandomRange);

    }
    public override void Update()
    {
        cadenceTime -= Time.deltaTime * enemy.timeMagnitude;
       
        enemy.GetComponentInChildren<gunRotate>().setRotation(enemy.detector.reachableObjects[0].transform.position);
        if(cadenceTime <= 0)
        {
            cadenceTime = enemy.shootCadence + Random.Range(-enemy.shootCadenceRandomRange,enemy.shootCadenceRandomRange);
            enemy.GetComponentInChildren<IShoot>().shoot();
        }
    }
}

public class StartChargeState : BaseEnemyState
{
    new HeavyEnemyController enemy;
    public StartChargeState(HeavyEnemyController _enemy)
    {
        enemy = _enemy;
    }

    public override void OnEnter()
    {
        Debug.Log("start StartCHarge");
        enemy.anim.Play("enemySpot");
        enemy.finishCharging = false;

        enemy.direction = enemy.detector.reachableObjects[0].transform.position.x > enemy.transform.position.x ? Vector3.right : Vector3.left;
        enemy.transform.localScale = new Vector3(-enemy.direction.x, 1, 1);

    }
}
public class ReloadState : BaseEnemyState
{
    new BossEnemyController enemy;
    public ReloadState(BossEnemyController _enemy)
    {
        enemy = _enemy;
    }

    public override void OnEnter()
    {
        enemy.GetComponentInChildren<IShoot>().getAnim().Play("gunReload");


    }
}
public class ChargeState : BaseEnemyState
{
    new HeavyEnemyController enemy;
    float speed;
    public ChargeState(HeavyEnemyController _enemy)
    {
        enemy = _enemy;
        speed = _enemy.speed;
    }

    public override void OnEnter()
    {
        enemy.anim.Play("enemyRun");
        enemy.wallDetect.gameObject.SetActive(true);
    }
    public override void Update()
    {
    }
    public override void FixedUpdate()
    {
        Debug.Log("fixed");
        enemy.rb2d.MovePosition(enemy.transform.position + speed * enemy.direction * Time.fixedDeltaTime * enemy.timeMagnitude);

    }
    public override void OnExit()
    {
        enemy.wallDetect.gameObject.SetActive(false);
    }
}
public class StunedState: BaseEnemyState
{
    public StunedState(EnemyController _enemy)
    {
        enemy = _enemy;
    }
    public override void Update()
    {

    }
    public override void OnEnter()
    {
enemy.stunedCollider.gameObject.SetActive(true);
        enemy.anim.Play("enemyStun");

    }
    public override void OnExit()
    {
        enemy.stunedCollider.gameObject.SetActive(false);
      
    }
}