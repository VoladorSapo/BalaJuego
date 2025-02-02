using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLife : MonoBehaviour
{
    [SerializeField] int currentLife;
    [SerializeField] int maxLife;
    public Team team;


  protected  Animator anim;

 protected   bool melee;

 [SerializeField]   GameObject spriteParent;


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            print("tag bullet");
            IBullet bul = collision.GetComponent<IBullet>();
            if (bul != null && (bul.getTeam() != team || bul.hurtAll() == true))
            {
                Damage(bul.getDamage());
                bul.hitSomething(gameObject);
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
        anim = GetComponentInChildren<Animator>();
    }
    public virtual void meleeDeath()
    {
        spriteParent.SetActive(false);
        melee = true;
    }
    public enum Team
    {
        Player,
        Enemy,
    }
    public virtual void restart()
    {
        currentLife = maxLife;
        melee = false;
    }
}
