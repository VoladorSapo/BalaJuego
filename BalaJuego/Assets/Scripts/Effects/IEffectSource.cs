
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectSource
{
    public void hitSomething(GameObject obj);


    public ACharacterLife.Team getTeam();
    public ACharacterLife getOwner();
    public HittableType getHittableType();


    public bool hurtAll();

    public GameObject getObj();
    public ACombatEffect[] getEffects();

    public void setInteractor(CharacterInteractor shooter);

    public void ActivateSource(bool activate);
}
public interface IProyectile
{
    public void ActivateProyectileMovement(CharacterInteractor shooter, float angle);
    public void ActivateProyectileMovement(CharacterInteractor shooter, float angle, Vector3 pos);
    public GameObject getObj();
    public void hitSomething(GameObject obj);
    void setActivateEvent(Action<CharacterInteractor> activateSource);
}