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
        enemy.GetComponentInChildren<CharacterShoot>().anim.Play("enemygunIdle");
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
        enemy.GetComponentInChildren<gunRotate>().setRotation(enemy.detector.reachableObjects[0].transform.position);
        if(cadenceTime <= 0)
        {
            cadenceTime = enemy.shootCadence;
            enemy.GetComponentInChildren<CharacterShoot>().shoot();
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
        enemy.anim.Play("enemySpot");
        enemy.finishCharging = false;
        enemy.direction = enemy.detector.reachableObjects[0].transform.position.x > enemy.transform.position.x ? Vector3.right : Vector3.left;

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
    }
    public override void Update()
    {
    }
    public override void FixedUpdate()
    {
        enemy.rb2d.MovePosition(enemy.transform.position + speed * enemy.direction * Time.fixedDeltaTime * enemy.timeMagnitude);

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

    }
    public override void OnExit()
    {
        enemy.stunedCollider.gameObject.SetActive(false);
      
    }
}