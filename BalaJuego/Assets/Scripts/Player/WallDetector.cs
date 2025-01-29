using UnityEngine;

public class WallDetector : MonoBehaviour
{
   public GameObject wall;
    private void Start()
    {
        wall = null;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Ground")
        {
            wall = collision.gameObject;
        }
    }
    public void restart()
    {
        wall = null;
    }
}