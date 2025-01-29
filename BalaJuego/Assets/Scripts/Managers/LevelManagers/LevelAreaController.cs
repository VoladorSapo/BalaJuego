using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAreaController : MonoBehaviour
{
 [SerializeField]   List<EnemyController> enemies;

  [SerializeField]  GameObject colliders;

    public void enemyDie(EnemyController enemy)
    {
        enemies.Remove(enemy);
        if(enemies.Count == 0)
        {
            colliders.SetActive(false);
        }
    }
    private void Start()
    {
        ServiceLocator.Instance.Get<ILevelController>().subscribeToRestart(restart);

        restart();
    }

    void restart()
    {
      foreach(EnemyController enem in enemies)
        {
            enem.gameObject.SetActive(true);
            enem.restart(this);
          
        }
    }
}
