using UnityEngine;

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

        enemy.direction = enemy.detectorManager.detectorDictionary["in"].getFirst().getObj().transform.position.x > enemy.transform.position.x ? Vector3.right : Vector3.left;
        enemy.transform.localScale = new Vector3(-enemy.direction.x, 1, 1);

    }
}


