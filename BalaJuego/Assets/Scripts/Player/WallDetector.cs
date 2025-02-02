using UnityEngine;

public class WallDetector : MonoBehaviour
{
  public GameObject wall;
    [SerializeField] LayerMask wallLayer;
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
    private void OnCollisionExit2D(Collision2D collision)
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
}