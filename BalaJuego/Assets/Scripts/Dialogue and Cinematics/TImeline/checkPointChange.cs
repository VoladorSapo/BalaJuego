using UnityEngine;

public class checkPointChange:MonoBehaviour{

   [SerializeField] Transform newPos;
    bool hasChanged;
    [SerializeField] PolygonCollider2D cameraConfinerCollider;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerMove>().changeSpawnPoint(newPos.position);
            FindObjectOfType<cameraController>().changeRestartConfiners(cameraConfinerCollider.points);
        }
    }
}