public class PlayerLife: ACharacterLife
{
    public override void Die()
    {
        musicManager.Instance.PlaySound("snd_damage");
        finishDeathAnim();
    }
    public void finishDeathAnim()
    {
        gameObject.SetActive(false);
        ServiceLocator.Instance.Get<ILevelController>().Lose();
    }
    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if(collision.gameObject.GetComponentInParent<HeavyEnemyController>() != null && !collision.gameObject.GetComponentInParent<EnemyLife>().dead && (!invincibility || !GetComponent<PlayerMove>().DEBUG_canDodgeHeavy))
        {
            Die();
        }
    }
}