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
          if(obj != null)  {
                print("Adding: " + collision.gameObject);
                reachableObjects.Add(obj);
            }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
      
            T obj = collision.GetComponent<T>();
            if (obj != null)
            {
                print("Removing: " + collision.gameObject);

                reachableObjects.Remove(obj);
            }
        
    }

}
