using UnityEngine;

public class TutorialLife : ACharacterLife
{
    public bool blockKill;
    [SerializeField] GameObject gun;
    [SerializeField] GameObject head;
    [SerializeField] GameObject countText;
    [SerializeField] Vector3 bodyMovePos;

    public override void Die()
    {
        if (!blockKill)
        {
            dead = true;
            GetComponent<EnemyBehaviour>().stunedCollider.enabled = false;
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
        if (melee)
        {

            PlayerMove player = FindObjectOfType<PlayerMove>();
            transform.position = player.transform.position + new Vector3(bodyMovePos.x * -player.transform.localScale.x, bodyMovePos.y, bodyMovePos.z);
            transform.localScale = new Vector3(player.runningDirection, 1, 1);
        }
        if (!blockKill)
        {
            Debug.LogError("Die");
            GetComponent<EnemyBehaviour>().setColor(false);
            //GetComponent<EnemyController>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}