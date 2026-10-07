using UnityEngine;

public class EnemyHeavyChargeState : BaseEnemyState
{
    new HeavyEnemyBehaviour enemy;
    float speed;
    public EnemyHeavyChargeState(HeavyEnemyBehaviour _enemy)
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
