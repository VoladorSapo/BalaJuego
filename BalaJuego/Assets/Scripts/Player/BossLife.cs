using UnityEngine;

public class BossLife: ACharacterLife
{
    public override void Die()
    {

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            
            print("tag bullet");
            IProyectile bul = collision.GetComponent<IProyectile>();
            if (bul != null && (bul.getTeam() != team || bul.hurtAll() == true))
            {
                anim.Play("enemyDodge");
                //esquivar
                // anim.Play();
            }
        }
    }
}