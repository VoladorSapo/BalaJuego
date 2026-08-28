using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ACharacterLife : MonoBehaviour, IHittable
{
    [SerializeField] int currentLife;
    [SerializeField] int maxLife;
    public bool dead;

    public Team team;
IGameState _gameStateManager;

    [SerializeField]protected  Animator anim;

    [SerializeField] protected bool melee;

 [SerializeField] protected  GameObject spriteParent;
    List<ACombatEffect> activeEffects;

    [SerializeField] bool CorpseBlockProjectile = false;
    public float timeMagnitude { get; private set; }

    [SerializeField] protected bool invincibility;

   public CharacterHat characterHat { get; private set; }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {

    }
    public void Damage(int damage)
    {
        if (!characterHat || characterHat.getHit())
        {
            currentLife -= damage;
            if (currentLife <= 0)
            {
                Die();
            }
        }
    }
    private void Update()
    {
        if (_gameStateManager.getState() == IGameState.gameState.NormalTime || _gameStateManager.getState() == IGameState.gameState.SlowDown)
        {
            for (int i = activeEffects.Count-1; i >=0; i--)
            {
                ACombatEffect effect = activeEffects[i];
                effect.Update(Time.deltaTime * timeMagnitude);
                if (effect.IsOver())
                {
                    effect.End();
                    activeEffects.RemoveAt(i);
                }
            }
        }
    }

    public abstract void Die();
    private void Start()
    {
        _gameStateManager = ServiceLocator.Instance.Get<IGameState>();
        activeEffects = new List<ACombatEffect>();
        print("characterHat"+name);
        characterHat = GetComponent<CharacterHat>();
        anim = GetComponentInChildren<Animator>();
        ServiceLocator.Instance.Get<ITimeManager>().subscribeToTimeChange(changeTimeMagnitude);
        timeMagnitude = 1;
    }
    public virtual void meleeDeath()
    {
        spriteParent.SetActive(false);
        melee = true;
    }
    void changeTimeMagnitude(object sender, timeData data)
    {
        timeMagnitude = data.currentMagnitude;
    }
    public enum Team
    {
        Player,
        Enemy,
    }
    public virtual void restart()
    {
        dead = false;
        currentLife = maxLife;
        melee = false;
        spriteParent.SetActive(true);
        timeMagnitude = 1;
        activeEffects.Clear();
        if (characterHat)
        {
            characterHat.resetHat();
        }
    }
    public void checkEffect(ACombatEffect effect)
    {
        effect.Activate(this);
        print("addefect" + effect.GetType().Name);
        if (!effect.Instant())
        {
            print("addefecttolist" + effect.GetType().Name);

            activeEffects.Add(effect);
        }
    }
    public bool getHit(IProyectile proyectile)
    {
        if ((!dead || CorpseBlockProjectile) && !invincibility && HittableCheck.checkHit(this, proyectile.getOwner(), proyectile.getHittableType()))
        {
            foreach (var effect in proyectile.getEffects())
            {
                checkEffect(effect);
            }
            return true;
        }
        return false;
    }

    public virtual void getStuned()
    {

    }
    public virtual void endStun()
    {

    }

    public void setInvincibility(bool invincibility)
    {
        this.invincibility = invincibility;
    }
}
