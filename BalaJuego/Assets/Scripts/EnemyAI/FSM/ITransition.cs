using Unity.VisualScripting;

public interface ITransition
{
    IState To { get; }
    IPredicate Condition { get; }
    bool allowPreExit { get; }
    string Data {  get; }
}


public class Transition : ITransition
{
    public IState To { get; }


    public IPredicate Condition { get; }

    public string Data { get; }

    public bool allowPreExit {get;}

    public Transition(IState _to, IPredicate _cond,bool _allowPreExit, string data = "")
    {
        To = _to;
        Condition = _cond;
        Data = data;
        allowPreExit = _allowPreExit;
    }
}


