using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class ACombatEffect
{
    protected ACharacterLife owner;
    protected ACharacterLife character;
    public abstract ACombatEffect Clone();
    public abstract bool Instant();
   
    public abstract void Update(float passedTime);
    public abstract bool IsOver();


    public abstract void End();

    public void Activate(ACharacterLife character)
    {
        this.character = character;
        activateEffect();
    }
    public abstract void activateEffect();
    public ACharacterLife getCharacter() => character;
    public ACharacterLife getOwner() => owner;
}
