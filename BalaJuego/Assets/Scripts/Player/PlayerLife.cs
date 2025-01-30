public class PlayerLife: CharacterLife
{
    public override void Die()
    {
        finishDeathAnim();
    }
    public void finishDeathAnim()
    {
        gameObject.SetActive(false);
        ServiceLocator.Instance.Get<ILevelController>().Lose();
    }
}