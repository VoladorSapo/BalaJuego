using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLife : MonoBehaviour
{
    [SerializeField] int currentLife;
    [SerializeField] int maxLife;
    public bool dead;

    public Team team;


  [SerializeField]protected  Animator anim;

    [SerializeField] protected bool melee;

 [SerializeField] protected  GameObject spriteParent;


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet" && !dead)
        {
            print("tag bullet");
            IProyectile bul = collision.GetComponent<IProyectile>();
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
        dead = false;
        currentLife = maxLife;
        melee = false;
        spriteParent.SetActive(true);
    }
}
