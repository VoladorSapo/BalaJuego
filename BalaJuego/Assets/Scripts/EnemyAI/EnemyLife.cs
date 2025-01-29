public class EnemyLife : CharacterLife
{
    protected override void Die()
    {
        finishDeathAnim();
    }
    public void finishDeathAnim()
    {
        Destroy(this);
    }
}
