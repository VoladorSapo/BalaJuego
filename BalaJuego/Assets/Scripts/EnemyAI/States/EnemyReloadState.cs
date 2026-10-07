public class EnemyReloadState : BaseEnemyState
{
    new BossEnemyBehaviour enemy;
    public EnemyReloadState(BossEnemyBehaviour _enemy)
    {
        enemy = _enemy;
    }

    public override void OnEnter()
    {
        enemy.GetComponentInChildren<IGun>().getAnim().Play("gunReload");


    }
}
