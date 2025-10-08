using UnityEngine;

public class EnemyLife : CharacterLife
{
    [SerializeField] GameObject gun;
    [SerializeField] GameObject head;
    [SerializeField] GameObject countText;
    [SerializeField] ParticleSystem hitParticles;
    [SerializeField] Vector3 bodyMovePos;
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
            musicManager.Instance.StopHeavyWalking();

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
            hitParticles.Play();
            //Animacion morir
        }
    }
    public void finishDeathAnim()
    {
        if (melee)
        {
            PlayerMove player = FindObjectOfType<PlayerMove>();
            transform.position = player.transform.position + new Vector3(bodyMovePos.x * -player.transform.localScale.x, bodyMovePos.y, bodyMovePos.z);
            transform.localScale = new Vector3(player.runningDirection, 1, 1);
            GetComponent<Rigidbody2D>().isKinematic = true;
        }
        if (GetComponent<EnemyController>().area != null)
        {
            GetComponent<EnemyController>().area.enemyDie(GetComponent<EnemyController>());
        }
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
    //int checkPlayerDir()
    //{
    //    if(FindObjectOfType<PlayerMove>().transform.position.x > gameObject.transform.position.x)
    //    {
    //        return 1;
    //    }
    //    return -1;
    //}
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
