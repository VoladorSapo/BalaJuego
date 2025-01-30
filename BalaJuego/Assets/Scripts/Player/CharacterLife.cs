using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLife : MonoBehaviour
{
    [SerializeField] int currentLife;
    [SerializeField] int maxLife;
    public Team team;


    Animator anim;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            print("tag bullet");
            IBullet bul = collision.GetComponent<IBullet>();
            if (bul != null && (bul.getTeam() != team || bul.hurtAll() == true))
            {
                Damage(bul.getDamage());
                bul.hitSomething();
            }
        }
    }
    public void Damage(int damage)
    {
        currentLife -= damage;
        if (currentLife <= 0)
        {
            Die();
        }
    }
    public virtual void Die()
    {
      //  anim.Play("Die");
        //finishDeath();
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public enum Team
    {
        Player,
        Enemy,
    }

}
