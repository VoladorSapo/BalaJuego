using UnityEngine;

public static class ObjectExtensions
{
    public static GameObject ToGameObject(this object o) => o switch
    {
        IDetectable d => d.getObj(),
        MonoBehaviour m => m.gameObject,
        Component c => c.gameObject,
        _ => null
    };
}