using UnityEngine;

public class EnemyLife : CharacterLife
{
    [SerializeField] GameObject gun;
    [SerializeField] GameObject head;
    [SerializeField] GameObject countText;
    public override void Die()
    {
        dead = true;
        GetComponent<EnemyController>().stunedCollider.enabled = false;
        // collider.gameObject.SetActive(false);
        if (melee)
        {

            spriteParent.SetActive(true);
            if (GetComponent<GunEnemyController>() != null)
            {
                countText.SetActive(false);
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
                countText.SetActive(false);
                gun.SetActive(false);
                head?.SetActive(false);
            }
            if (GetComponent<HeavyEnemyController>() != null)
            {
                head?.SetActive(false);
            }
            anim.Play("enemyDie");

            //Animacion morir
        }
    }
    public void finishDeathAnim()
    {
        GetComponent<EnemyController>().area.enemyDie(GetComponent<EnemyController>());
        GetComponent<EnemyController>().setColor(false);

        GetComponent<EnemyController>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        // gameObject.SetActive(false);
    }
    public override void restart()
    {
        base.restart();

        if (GetComponent<GunEnemyController>() != null)
        {
            print("countext" + gameObject.name);
            countText.SetActive(true);
            gun.SetActive(true);
            head?.SetActive(true);
        }
        if (GetComponent<HeavyEnemyController>() != null)
        {
            head?.SetActive(true);
        }
        GetComponent<EnemyController>().enabled = true;
        GetComponent<EnemyController>().setColor(false);
        GetComponent<EnemyController>().stunedCollider.enabled = true;


    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        print("trigger");
        if (collision.tag == "Botella" && !dead)
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
