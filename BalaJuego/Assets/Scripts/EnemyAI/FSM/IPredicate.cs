using System;
using System.Collections;
using System.Collections.Generic;

public interface IPredicate
{
    bool Evaluate();
}
public class FuncPredicate : IPredicate
{
    readonly Func<bool> func;
    public FuncPredicate(Func<bool> _func)
    {
        func = _func;
    }
    public bool Evaluate() => func.Invoke();
}
