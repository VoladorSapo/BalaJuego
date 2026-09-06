using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyIdleState : BaseEnemyState
{
    public EnemyIdleState(AEnemyBehaviour _enemy)
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
    public EnemyShootState(AEnemyBehaviour _enemy)
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
    new HeavyEnemyBehaviour enemy;
    public StartChargeState(HeavyEnemyBehaviour _enemy)
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


