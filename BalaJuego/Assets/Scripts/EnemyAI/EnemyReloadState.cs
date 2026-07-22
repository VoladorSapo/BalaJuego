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
