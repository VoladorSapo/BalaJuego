using UnityEngine;

public class StartChargeState : EnemyPlayAnimationState
{
    new HeavyEnemyBehaviour enemy;
    public StartChargeState(HeavyEnemyBehaviour _enemy): base(_enemy,"Spot")
    {
        enemy = _enemy;
    }

    public override void OnEnter()
    {
        base.OnEnter();
     

        enemy.direction = enemy.detectorManager.detectorDictionary["in"].getFirst().ToGameObject().transform.position.x > enemy.transform.position.x ? Vector3.right : Vector3.left;
        enemy.transform.localScale = new Vector3(-enemy.direction.x, 1, 1);

    }
}


