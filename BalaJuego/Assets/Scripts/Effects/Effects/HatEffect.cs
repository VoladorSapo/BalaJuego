public class HatEffect : AInstantEffect
{
    public int _hatShield { get; protected set; }
    public HatEffect(int shield)
    {
        _hatShield = shield;
    }
    public HatEffect() { }

    public override void activateEffect()
    {
        character.characterHat.setLives(_hatShield);
    }

    public override ACombatEffect Clone()
    {
        HatEffect clone = new HatEffect();
        clone._hatShield = _hatShield;
        clone.owner = owner;
        clone.character = character;
        return clone;
    }
}
