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
        if (enemy.GetComponentInChildren<IGun>() != null)
        {
            enemy.GetComponentInChildren<IGun>().getAnim().Play("gunIdle");
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
        enemy.GetComponentInChildren<IGun>().setShooting(false);
        enemy.anim.Play("enemySpot");
        enemy.GetComponentInChildren<IGun>().getAnim().Play("enemyGunSpot");
        cadenceTime = enemy.shootOnShight ? 0.1f : enemy.shootCadence + Random.Range(-enemy.shootCadenceRandomRange, enemy.shootCadenceRandomRange);

    }
    public override void Update()
    {
        cadenceTime -= Time.deltaTime * enemy.timeMagnitude;
       
        enemy.GetComponentInChildren<gunRotate>().setRotation(enemy.detector.reachableObjects[0].transform.position);
        if(cadenceTime <= 0)
        {
            cadenceTime = enemy.shootCadence + Random.Range(-enemy.shootCadenceRandomRange,enemy.shootCadenceRandomRange);
            enemy.GetComponentInChildren<IGun>().shoot();
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
        enemy.GetComponentInChildren<IGun>().getAnim().Play("gunReload");


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
        musicManager.Instance.StartHeavyWalking();
        enemy.anim.Play("enemyRun");
        enemy.wallDetect.gameObject.SetActive(true);
    }
    public override void Update()
    {
    }
    public override void FixedUpdate()
    {
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
        musicManager.Instance.StopHeavyWalking();
        enemy.stunedCollider.gameObject.SetActive(true);
        enemy.anim.Play("enemyStun");
        if(enemy.GetComponent<HeavyEnemyController>() != null)
        {
            enemy.GetComponentInChildren<WallDetector>().gameObject.SetActive(false);
        }

    }
    public override void OnExit()
    {
        enemy.stunedCollider.gameObject.SetActive(false);
      
    }
}