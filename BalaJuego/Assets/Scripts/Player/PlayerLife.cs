public class PlayerLife: CharacterLife
{
    protected override void Die()
    {
        finishDeathAnim();
    }
    public void finishDeathAnim()
    {
        ServiceLocator.Instance.Get<IGameState>().Die();
    }
}