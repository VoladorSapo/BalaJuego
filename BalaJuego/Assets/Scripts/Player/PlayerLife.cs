public class PlayerLife: CharacterLife
{
    protected override void Die()
    {
        finishDeathAnim();
    }
    public void finishDeathAnim()
    {
        gameObject.SetActive(false);
        ServiceLocator.Instance.Get<ILevelController>().Lose();
    }
}