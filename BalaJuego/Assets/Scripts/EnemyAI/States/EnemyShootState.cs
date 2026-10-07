using UnityEngine;

public class EnemyShootState : BaseEnemyState
{

    float cadenceTime;
    Transform playerTransform;
    public EnemyShootState(AEnemyBehaviour _enemy)
    {
        enemy = _enemy;
    }

    public override void OnEnter()
    {
        enemy.playAnimation("Spot");
        enemy.GetComponentInChildren<IGun>().setShooting(false);
        cadenceTime = enemy.differentFirstShootCadence ? enemy.firstShootCadence : enemy.shootCadence + Random.Range(-enemy.shootCadenceRandomRange, enemy.shootCadenceRandomRange);
        playerTransform = enemy.detectorManager.detectorDictionary["in"].getFirst().getObj().transform;
    }
    public override void Update()
    {
        cadenceTime -= Time.deltaTime * enemy.timeMagnitude;
        if (enemy.DebugOn)
        {
            LogValue("cadenceTime",cadenceTime.ToString());
        }
        enemy.GetComponentInChildren<gunRotate>().setRotation(playerTransform.position);
        if(cadenceTime <= 0)
        {
            LogDebug("shoot");
            cadenceTime = enemy.shootCadence + Random.Range(-enemy.shootCadenceRandomRange,enemy.shootCadenceRandomRange);
            enemy.GetComponentInChildren<IGun>().shoot();
        }
    }
}


