using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLife : MonoBehaviour
{
    [SerializeField] int currentLife;
    [SerializeField] int maxLife;

    Animator anim;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            IBullet bul = collision.GetComponent<IBullet>();
            if (bul != null)
            {
                Damage(bul.getDamage());               
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
    protected void Die()
    {
        anim.Play("Die");
    }
    public void finishDeath()
    {
        Destroy(gameObject);
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
}
