using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyIdleState : BaseEnemyState
{
    public EnemyIdleState(EnemyBehaviour _enemy)
    {
        enemy = _enemy;
    }

    public override void OnEnter()
    { 
        Debug.Log("start Idle");
        enemy.playAnimation("Idle");
    }
    public override void Update()
    {
    }
}

public class EnemyShootState : BaseEnemyState
{

    float cadenceTime;
    public EnemyShootState(EnemyBehaviour _enemy)
    {
        enemy = _enemy;
    }

    public override void OnEnter()
    {
        enemy.GetComponentInChildren<IGun>().setShooting(false);
        enemy.playAnimation("Spot");
        cadenceTime = enemy.differentFirstShootCadence ? enemy.firstShootCadence : enemy.shootCadence + Random.Range(-enemy.shootCadenceRandomRange, enemy.shootCadenceRandomRange);

    }
    public override void Update()
    {
        cadenceTime -= Time.deltaTime * enemy.timeMagnitude;
        if (enemy.DebugOn)
        {
            LogValue("cadenceTime",cadenceTime.ToString());
        }
        enemy.GetComponentInChildren<gunRotate>().setRotation(enemy.detector.reachableObjects[0].transform.position);
        if(cadenceTime <= 0)
        {
            LogDebug("shoot");
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
        enemy.playAnimation("Spot");
        enemy.finishCharging = false;

        enemy.direction = enemy.detector.reachableObjects[0].transform.position.x > enemy.transform.position.x ? Vector3.right : Vector3.left;
        enemy.transform.localScale = new Vector3(-enemy.direction.x, 1, 1);

    }
}
public class EnemyReloadState : BaseEnemyState
{
    new BossEnemyController enemy;
    public EnemyReloadState(BossEnemyController _enemy)
    {
        enemy = _enemy;
    }

    public override void OnEnter()
    {
        enemy.GetComponentInChildren<IGun>().getAnim().Play("gunReload");


    }
}
public class EnemyHeavyChargeState : BaseEnemyState
{
    new HeavyEnemyController enemy;
    float speed;
    public EnemyHeavyChargeState(HeavyEnemyController _enemy)
    {
        enemy = _enemy;
        speed = _enemy.speed;
    }

    public override void OnEnter()
    {
        musicManager.Instance.StartHeavyWalking();
        enemy.playAnimation("Run");
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
public class EnemyStunedState: BaseEnemyState
{
    public EnemyStunedState(EnemyBehaviour _enemy)
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
        enemy.playAnimation("Stun");
        if(enemy.GetComponent<HeavyEnemyController>() != null)
        {
            enemy.GetComponentInChildren<WallDetector>().gameObject.SetActive(false);
        }

    }
    public override void OnExit()
    {
        Debug.Log("end stun");
        enemy.stunedCollider.gameObject.SetActive(false);
      
    }
}