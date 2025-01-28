using UnityEngine;

public class IdleState : BaseEnemyState
{
    public IdleState(EnemyController _enemy)
    {
        enemy = _enemy;
    }

    public override void OnEnter()
    {
        enemy.anim.Play("enemyIdle");
    }
    public override void Update()
    {
        Debug.Log("idle");
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
        enemy.anim.Play("enemySpot");
        enemy.GetComponentInChildren<CharacterShoot>().anim.Play("enemyGunSpot");
        cadenceTime = enemy.shootCadence;

    }
    public override void Update()
    {
        cadenceTime -= Time.deltaTime * enemy.timeMagnitude;
        Debug.Log(cadenceTime);
        enemy.GetComponentInChildren<gunRotate>().setRotation(enemy.detector.reachableObjects[0].transform.position);
        if(cadenceTime <= 0)
        {
            cadenceTime = enemy.shootCadence;
            enemy.GetComponentInChildren<CharacterShoot>().shoot();
        }
    }
}