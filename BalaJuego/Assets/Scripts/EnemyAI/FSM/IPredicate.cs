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
    public virtual bool Evaluate() => func.Invoke();
}

public class AndPredicate : IPredicate
{
   protected readonly IPredicate[] funcs;
    public AndPredicate(params IPredicate[] predicates)
    {
        funcs = predicates;
    }
    public virtual bool Evaluate() {
        foreach (var pred in funcs)
        {
            if (!pred.Evaluate()) return false;
        }
        return true;
    
    }
}

public class OrPredicate : AndPredicate
{
    public OrPredicate(IPredicate[] predicates) : base(predicates)
    {
    }

    public override bool Evaluate()
    {
        foreach (var pred in funcs)
        {
            if (pred.Evaluate()) return true;
        }
        return false;
    }
}

public class NotPredicate : IPredicate
{
    IPredicate predicate;
    public NotPredicate(IPredicate _func)
    {
        predicate = _func;
    }
    public bool Evaluate()
    {
        return !predicate.Evaluate();
    }
}
public class TruePredicate : IPredicate
{
    public bool Evaluate() => true;
}