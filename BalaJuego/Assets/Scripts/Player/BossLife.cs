using UnityEngine;

public class BossLife: CharacterLife
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            
            print("tag bullet");
            IBullet bul = collision.GetComponent<IBullet>();
            if (bul != null && (bul.getTeam() != team || bul.hurtAll() == true))
            {
                anim.Play("enemyDodge");
                //esquivar
                // anim.Play();
            }
        }
    }
}