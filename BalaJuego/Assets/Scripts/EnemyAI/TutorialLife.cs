public class TutorialLife : CharacterLife
{
    public override void Die()
    {
        finishDeathAnim();
    }
    public void finishDeathAnim()
    {
        Destroy(gameObject);
    }
}