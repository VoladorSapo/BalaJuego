using Unity.VisualScripting;

public interface ITransition
{
    IState To { get; }
    IPredicate Condition { get; }

    string Data {  get; }
}


public class Transition : ITransition
{
    public IState To { get; }


    public IPredicate Condition { get; }

    public string Data { get; }

    public Transition(IState _to, IPredicate _cond, string data = "")
    {
        To = _to;
        Condition = _cond;
        Data = data;
    }
}


