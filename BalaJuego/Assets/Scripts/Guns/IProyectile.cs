
using UnityEngine;

public interface IProyectile
{
    public void InstantiateBullet(CharacterLife shooter, float angle);
    public void InstantiateBullet(CharacterLife shooter, float angle, Vector3 pos);


    public int getDamage();

    public void hitSomething(GameObject obj);

    public CharacterLife.Team getTeam();

    public bool hurtAll();

    public GameObject getObj();

}

public interface IInteractable
{
    public void tryGrab(PlayerShoot player);

    public void setHover(bool set);

    public GameObject getObj();
}

public interface IEquipable
{
    public void Action(CharacterLife shooter, float angle);
    public GameObject getObj();

}