using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAreaController : MonoBehaviour
{
    List<EnemyController> enemies;

    GameObject colliders;

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
    }

    void restart()
    {
      foreach(EnemyController enem in enemies)
        {
            enem.gameObject.SetActive(true);
        }
    }
}
