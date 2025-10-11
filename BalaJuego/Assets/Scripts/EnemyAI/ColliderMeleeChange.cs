using UnityEngine;

public class ColliderMeleeChange : MonoBehaviour
{
  [SerializeField]  EnemyController enemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("collision" + collision.tag);
        if(collision.tag == "Player")
        {
            print("player");

            enemy.canBeKilledMelee = false;
        }
    }
}
