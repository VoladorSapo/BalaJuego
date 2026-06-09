
using UnityEngine;

public interface IProyectile
{
    public void InstantiateBullet(CharacterLife shooter, float angle);

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