using UnityEngine;

public class EnemyLife : CharacterLife
{
    public override void Die()
    {
        if (melee)
        {
            //Sprite muerto melee
            finishDeathAnim();
        }
        else
        {
            //Animacion morir
        }
        finishDeathAnim();
    }
    public void finishDeathAnim()
    {
        GetComponent<EnemyController>().area.enemyDie(GetComponent<EnemyController>());
        gameObject.SetActive(false);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        print("trigger");
        if (collision.tag == "Botella")
        {
            print("triggerBotella");
            botella botel = collision.GetComponent<botella>();
            if (botel.isThrown)
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
