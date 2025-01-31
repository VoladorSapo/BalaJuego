using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelAreaController : MonoBehaviour
{
 [SerializeField]   List<EnemyController> enemies;

  [SerializeField]  GameObject colliders;

    CinemachineVirtualCamera virtCamera;

    bool started;
  [field:SerializeField] public  BoxCollider2D startCollider { get; private set; }
    [field: SerializeField] public BoxCollider2D endCollider { get; private set; }

    [field: SerializeField] public BoxCollider2D startTrigger { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(  collision.tag == "Player")
        {
            print("hey");
            started = true;
            ServiceLocator.Instance.Get<ILevelController>().startArea(this);
            
            startCollider.gameObject.SetActive(true);
            endCollider.gameObject.SetActive(true);
           startTrigger.gameObject.SetActive(false);

            enemyDie(null);

        }
    }
    public void enemyDie(EnemyController enemy)
    {
        if (enemy != null)
        {
            enemies.Remove(enemy);
        }
        if(started && enemies.Count == 0)
        {
            endCollider.gameObject.SetActive(false);
            startTrigger.gameObject.SetActive(false);
            started = false;
            ServiceLocator.Instance.Get<ILevelController>().endArea(this);
        }
    }
    private void Start()
    {
        ServiceLocator.Instance.Get<ILevelController>().subscribeToRestart(restart);
    }

    void restart()
    {
        started = false;
      foreach(EnemyController enem in enemies)
        {
            enem.gameObject.SetActive(true);
            enem.restart(this);
        }
        print("restae");
        startTrigger.gameObject.SetActive(true);
        startCollider.gameObject.SetActive(false);
        endCollider.gameObject.SetActive(false);



    }
}
