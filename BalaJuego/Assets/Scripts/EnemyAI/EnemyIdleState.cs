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
        enemy.anim.Play("enemySpot");
        enemy.GetComponentInChildren<IGun>().getAnim().Play("enemyGunSpot");
        cadenceTime = enemy.differentFirstShootCadence ? enemy.firstShootCadence : enemy.shootCadence + Random.Range(-enemy.shootCadenceRandomRange, enemy.shootCadenceRandomRange);

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
