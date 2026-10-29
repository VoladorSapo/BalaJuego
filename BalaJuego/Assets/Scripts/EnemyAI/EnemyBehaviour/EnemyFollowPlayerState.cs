internal class EnemyFollowPlayerState: BaseEnemyState
{
    private KnifeEnemyBehaviour knifeEnemyBehaviour;

    public EnemyFollowPlayerState(KnifeEnemyBehaviour knifeEnemyBehaviour)
    {
        this.knifeEnemyBehaviour = knifeEnemyBehaviour;
    }
}