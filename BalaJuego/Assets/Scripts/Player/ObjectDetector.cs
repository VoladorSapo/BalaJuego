using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector<T> : ObjectDetectorBase where T: class
{
 public   List<T> reachableObjects;
    public bool debugLogs = false;
    public override IReadOnlyList<object> getObjects() => reachableObjects;

    [SerializeField] LayerMask layer = ~0;


    private void Start()
    {
        reachableObjects = new List<T>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        T obj;

        if (detectParent)
        {
            obj = collision.GetComponentInParent<T>();

        }
        else
        {
            obj = collision.GetComponent<T>();
        }
        if (obj != null && (layer & (1 << collision.gameObject.layer)) != 0)
        {
            if (debugLogs)
            {
                print("Adding: " + collision.gameObject + name);
            }
            detectSomethingAction.Invoke(obj.ToGameObject());
            reachableObjects.Insert(0, obj);
            Hover(obj);
            BecomeFirst(obj);
            if (reachableObjects.Count > 1)
            {
                UnBecomeFirst(reachableObjects[1]);
            }
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        T obj;
        if (detectParent)
        {
            obj = collision.GetComponentInParent<T>();

        }
        else
        {
            obj = collision.GetComponent<T>();
        }
        if (obj != null)
        {
            UnHover(obj);
            bool wasFirst = false;
            if (reachableObjects.IndexOf(obj) == 0)
            {
                wasFirst = true;
                UnBecomeFirst(obj);
            }
            if (debugLogs)
            {
                print("Removing: " + collision.gameObject + name);
            }
            reachableObjects.Remove(obj);
            if (reachableObjects.Count > 0 && wasFirst)
            {
                BecomeFirst(reachableObjects[0]);
            }
        }
        
    }
    public override void restart()
    {
        reachableObjects.Clear();
        GetComponent<Collider2D>().enabled = false;

        GetComponent<Collider2D>().enabled = true;
    }
    public virtual void Hover(T obj)
    {

    }
    public virtual void UnHover(T obj)
    {

    }
    public virtual void BecomeFirst(T obj)
    {

    }
    public virtual void UnBecomeFirst(T obj)
    {

    }

    public override object getFirst()
    {
        if(reachableObjects.Count == 0)
        {
            return null;
        }
        else
        {
            return reachableObjects[0];
        }
    }

    public override int getCount()=>reachableObjects.Count;
}
public abstract class ObjectDetectorBase : MonoBehaviour
{
   protected Action<object> detectSomethingAction = (object o )=> { };
    [SerializeField] protected bool detectParent;
    public abstract IReadOnlyList<object> getObjects();
    public abstract object getFirst();
    public abstract int getCount();


    public abstract void restart();

    public void addDetectEvent(Action<object> action)
    {
        detectSomethingAction += action;
    }

}

public interface IDetectable
{
    public GameObject getObj();
}