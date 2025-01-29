public class EnemyLife : CharacterLife
{
    protected override void Die()
    {
        finishDeathAnim();
    }
    public void finishDeathAnim()
    {
        GetComponent<EnemyController>().area.enemyDie(GetComponent<EnemyController>());
        gameObject.SetActive(false);
    }
}
