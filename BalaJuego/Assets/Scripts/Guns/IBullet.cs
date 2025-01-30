
using UnityEngine;

public interface IBullet
{
    public void InstantiateBullet(CharacterLife shooter, float angle);

    public int getDamage();

    public void hitSomething(GameObject obj);

    public CharacterLife.Team getTeam();

    public bool hurtAll();

    public void tryGrab(PlayerShoot player);

    //public bool damageType
    //{
    //    hurtOtherTeam,
    //    hurtAll
    //}
}
