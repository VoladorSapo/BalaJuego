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
public abstract class ATimeBasedEffect : ACombatEffect
{
    public abstract float getDuration();
    protected float _currentDuration;

    public ATimeBasedEffect()
    {
        _currentDuration = getDuration();
    }
    public override void Update(float passedTime)
    {
        _currentDuration -= passedTime;
    }
    public override bool IsOver()
    {
        return _currentDuration <= 0;
    }
    public override bool Instant()
    {
        return false;
    }
}
public abstract class AInstantEffect : ACombatEffect
{
    public override void End()
    {
        return;
    }

    public override bool Instant()
    {
        return true;
    }

    public override bool IsOver()
    {
        return true;
    }
    public override void Update(float passedTime)
    {
        return;
    }
}
public class DamageEffect : AInstantEffect
{
    public int _damage { get; protected set; }
    public DamageEffect(int damage)
    {
        _damage = damage;
    }
    public DamageEffect() { }

    public override void activateEffect()
    {
        character.Damage(_damage);
    }

    public override ACombatEffect Clone()
    {
        DamageEffect clone = new DamageEffect();
        clone._damage = _damage;
        clone.owner = owner;
        clone.character = character;
        return clone;
    }
}
public class StunEffect : ATimeBasedEffect
{
    public float stunDuration { get; protected set; }

    public StunEffect(float duration)
    {
        stunDuration = duration;
    }
    public StunEffect()
    {

    }
    public override ACombatEffect Clone()
    {
        StunEffect clone = new StunEffect();
        clone.stunDuration = stunDuration;
        clone.owner = owner;
        clone.character = character;
        return clone;
    }
    public override void End()
    {
        character.endStun();
    }
    public override float getDuration()
    {
        return stunDuration;
    }
    public override void activateEffect()
    {
        _currentDuration = getDuration();
        character.getStuned();
    }
}