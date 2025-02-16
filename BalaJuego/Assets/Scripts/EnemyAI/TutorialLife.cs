using UnityEngine;

public class TutorialLife : CharacterLife
{
    public bool blockKill;
    [SerializeField] GameObject gun;
    [SerializeField] GameObject head;
    [SerializeField] GameObject countText;
    public override void Die()
    {
        if (!blockKill)
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
                Debug.LogError("Morir");
               anim.Play("enemyDie");

                //Animacion morir
            }


        }
    }
    public void finishDeathAnim()
    {
        if (!blockKill)
        {
            Debug.LogError("Die");
            GetComponent<EnemyController>().setColor(false);
            //GetComponent<EnemyController>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}