using UnityEngine.Timeline;

public class ParameterizedEmitter<T> : SignalEmitter
{
    public T parameter;
}
public class ParameterizedEmmiterTwo<T,T2> : SignalEmitter
{
    public T parameter;
    public T2 secondParameter;
}