using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector<T> : ObjectDetectorBase where T:IDetectable
{
 public   List<IDetectable> reachableObjects;

    public override List<IDetectable> getObjects() => reachableObjects;

    private void Start()
    {
        reachableObjects = new List<IDetectable>();
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
        if (obj != null)
        {
            print("Adding: " + collision.gameObject +name);
            reachableObjects.Insert(0, obj);
            Hover(obj);
            BecomeFirst(obj);
            if (reachableObjects.Count > 1)
            {
                UnBecomeFirst(reachableObjects[1].getObj().GetComponent<T>());
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
            print("Removing: " + collision.gameObject+name);

            reachableObjects.Remove(obj);
            if (reachableObjects.Count > 0 && wasFirst)
            {

                BecomeFirst(obj);
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

    public override IDetectable getFirst()
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
    [SerializeField] protected bool detectParent;
    public abstract List<IDetectable> getObjects();
    public abstract IDetectable getFirst();
    public abstract int getCount();


    public abstract void restart();

}
//public class ObjectParentDetector<T> : ObjectDetectorBase where T : MonoBehaviour,IDetectable
//{
//    public List<IDetectable> reachableObjects;

//    private void Start()
//    {
//        reachableObjects = new List<IDetectable>();
//    }
//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        //  print("hey" + collision.name);
//        T obj = collision.GetComponentInParent<T>();
//        print(obj);
//        if (obj != null)
//        {
//            //    print("Adding: " + collision.gameObject);
//            reachableObjects.Insert(0, obj);
//            Hover(obj);
//            BecomeFirst(obj);
//            if (reachableObjects.Count > 1)
//            {
//                UnBecomeFirst(reachableObjects[1].getObj().GetComponent<T>());
//            }
//        }

//    }
//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        //  print("ontriggerexitparent");
//        T obj = collision.GetComponentInParent<T>(true);
//        print(obj);
//        if (obj != null)
//        {
//            // print("Removing: " + collision.gameObject);
//            UnHover(obj);
//            bool wasFirst = false;
//            if (reachableObjects.IndexOf(obj) == 0)
//            {
//                wasFirst = true;
//                UnBecomeFirst(obj);
//            }
//            reachableObjects.Remove(obj);
//            if (reachableObjects.Count > 0 && wasFirst)
//            {

//                BecomeFirst(obj);
//            }
//        }

//    }
//    public virtual void Hover(T obj)
//    {

//    }
//    public virtual void UnHover(T obj)
//    {

//    }
//    public virtual void BecomeFirst(T obj)
//    {

//    }
//    public virtual void UnBecomeFirst(T obj)
//    {

//    }

//    public override List<IDetectable> getObject() => reachableObjects;
//}

public interface IDetectable
{
    public GameObject getObj();
}