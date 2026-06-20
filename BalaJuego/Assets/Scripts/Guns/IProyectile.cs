
using System.Collections.Generic;
using UnityEngine;

public interface IProyectile
{
    public void InstantiateBullet(ACharacterLife shooter, float angle);
    public void InstantiateBullet(ACharacterLife shooter, float angle, Vector3 pos);


    public int getDamage();

    public void hitSomething(GameObject obj);


    public ACharacterLife.Team getTeam();
    public ACharacterLife getOwner();
    public HittableType getHittableType();


    public bool hurtAll();

    public GameObject getObj();
    public ACombatEffect getEffect();
}

public interface IInteractable
{
    public void tryGrab(PlayerShoot player);

    public void setHover(bool set);

    public GameObject getObj();
}

public interface IEquipable
{
    public void Action(ACharacterLife shooter, float angle);
    public GameObject getObj();

}
public interface IHittable
{
    public bool getHit(IProyectile proyectile);
    public void Damage(int damage);
    public void Die();
}