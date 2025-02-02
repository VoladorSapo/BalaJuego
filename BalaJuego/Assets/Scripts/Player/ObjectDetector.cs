using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector<T> : MonoBehaviour
{
 public   List<T> reachableObjects;

    private void Start()
    {
        reachableObjects = new List<T>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
         T obj = collision.GetComponent<T>();
        if (obj != null)
        {
            print("Adding: " + collision.gameObject);
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
      
            T obj = collision.GetComponent<T>();
        if (obj != null)
        {
            print("Removing: " + collision.gameObject);
            UnHover(obj);
            bool wasFirst = false;
            if (reachableObjects.IndexOf(obj) == 0)
            {
                wasFirst = true;
                UnBecomeFirst(obj);
            }
            reachableObjects.Remove(obj);
            if (reachableObjects.Count > 0 && wasFirst)
            {

                BecomeFirst(obj);
            }
        }
        
    }
    public void restart()
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
}

public class ObjectParentDetector<T> : MonoBehaviour
{
    public List<T> reachableObjects;

    private void Start()
    {
        reachableObjects = new List<T>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("hey" + collision.name);
        T obj = collision.GetComponentInParent<T>();
        print(obj);
        if (obj != null)
        {
            print("Adding: " + collision.gameObject);
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
        print("ontriggerexitparent");
        T obj = collision.GetComponentInParent<T>(true);
        print(obj);
        if (obj != null)
        {
            print("Removing: " + collision.gameObject);
            UnHover(obj);
            bool wasFirst = false;
            if (reachableObjects.IndexOf(obj) == 0)
            {
                wasFirst = true;
                UnBecomeFirst(obj);
            }
            reachableObjects.Remove(obj);
            if (reachableObjects.Count > 0 && wasFirst)
            {

                BecomeFirst(obj);
            }
        }

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
}
