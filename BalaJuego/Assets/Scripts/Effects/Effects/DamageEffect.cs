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
