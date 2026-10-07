public class StunEffect : ATimeBasedEffect
{
    public float stunDuration { get; protected set; }

    public StunEffect(bool infinite,float duration)
    {
        _instant = infinite;
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