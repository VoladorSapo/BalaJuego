using System;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
  public GameObject wall;
    [SerializeField] LayerMask wallLayer;
    Action detectWallEvent;
    private void Start()
    {
        wall = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("trytouchwall");
        if (collision.transform.tag == "Ground")
        {
            wall = collision.gameObject;
            print("touchwall");
            detectWallEvent?.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ground" && wall == collision.gameObject)
        {
            wall = null;

        }
    }
    public void restart()
    {
        wall = null;
    }
    public void addWallDetectEvent(Action action)
    {
        detectWallEvent += action;
    }
}