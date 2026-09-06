
using System.Collections.Generic;
using UnityEngine;

public interface IProyectile
{
    public void ActivateProyectileMovement(ACharacterLife shooter, float angle);
    public void ActivateProyectileMovement(ACharacterLife shooter, float angle, Vector3 pos);


    public void hitSomething(GameObject obj);


    public ACharacterLife.Team getTeam();
    public ACharacterLife getOwner();
    public HittableType getHittableType();


    public bool hurtAll();

    public GameObject getObj();
    public ACombatEffect[] getEffects();
}
