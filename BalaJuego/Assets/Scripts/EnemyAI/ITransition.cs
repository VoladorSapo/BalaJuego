public interface ITransition
{
    IState To { get; }
    IPredicate Condition { get; }
}

public class Transition : ITransition
{
    public IState To { get; }


    public IPredicate Condition { get; }

    public Transition(IState _to, IPredicate _cond)
    {
        To = _to;
        Condition = _cond;
    }
}
