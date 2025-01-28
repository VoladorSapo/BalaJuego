
using UnityEngine;

public interface IBullet
{
    public void InstantiateBullet(CharacterLife shooter, float angle);

    public int getDamage();

    public void hitSomething();

    public CharacterLife.Team getTeam();

    public bool hurtAll();

    public void tryGrab();

    //public bool damageType
    //{
    //    hurtOtherTeam,
    //    hurtAll
    //}
}
