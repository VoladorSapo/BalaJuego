public abstract class ATimeBasedEffect : ACombatEffect
{
    public abstract float getDuration();
    protected float _currentDuration;
    protected bool _instant;

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
    public override bool Instant() => _instant;
}
