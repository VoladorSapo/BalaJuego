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
    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if(collision.gameObject.GetComponentInParent<HeavyEnemyController>() != null)
        {
            Die();
        }
    }
}