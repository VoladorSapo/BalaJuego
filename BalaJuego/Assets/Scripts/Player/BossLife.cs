using UnityEngine;

public class BossLife: CharacterLife
{
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