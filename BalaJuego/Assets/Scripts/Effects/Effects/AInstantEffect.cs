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
