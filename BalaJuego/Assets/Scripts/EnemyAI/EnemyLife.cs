using UnityEngine;

public class EnemyLife : CharacterLife
{
    [SerializeField] GameObject gun;
    [SerializeField] GameObject head;
    public override void Die()
    {
        dead = true;
        if (melee)
        {
            spriteParent.SetActive(true);

            if (GetComponent<GunEnemyController>() != null)
            {
                gun.SetActive(false);
                head?.SetActive(false);
            }
            //Sprite muerto melee
            anim.Play("enemyDeadMelee");
            finishDeathAnim();
        }
        else
        {
            if (GetComponent<GunEnemyController>() != null)
            {
                gun.SetActive(false);
                head?.SetActive(false);
            }
            anim.Play("enemyDie");

            //Animacion morir
        }
    }
    public void finishDeathAnim()
    {
        GetComponent<EnemyController>().area.enemyDie(GetComponent<EnemyController>());
       
        GetComponent<EnemyController>().enabled = false;

        // gameObject.SetActive(false);
    }
    public override void restart()
    {
        base.restart();
        if (GetComponent<GunEnemyController>() != null)
        {
            gun.SetActive(true);
            head?.SetActive(true);
        }
        GetComponent<EnemyController>().enabled = true;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        print("trigger");
        if (collision.tag == "Botella")
        {
            print("triggerBotella");
            baseBullet botel = collision.GetComponent<baseBullet>();
            if (botel.speed > 0)
            {
                GetComponent<EnemyController>().getStuned();
                botel.hitSomething(gameObject);
            }
            else
            {
                print("cagaste");
            }
        }

    }
}
