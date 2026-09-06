using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelAreaController : MonoBehaviour
{
    [SerializeField] public List<AEnemyBehaviour> enemies;
    [SerializeField] int aliveEnemies;

    [SerializeField] List<Throwable> botellas;
    public int intactBottles;

    [SerializeField] GameObject colliders;

    CinemachineVirtualCamera virtCamera;

    bool started;
    [field: SerializeField] public BoxCollider2D startCollider { get; private set; }
    [field: SerializeField] public BoxCollider2D endCollider { get; private set; }

    [field: SerializeField] public BoxCollider2D startTrigger { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
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
    public void enemyDie(AEnemyBehaviour enemy)
    {
        if (enemy != null && enemy.gameObject.activeSelf)
        {
            aliveEnemies--;
            ServiceLocator.Instance.Get<IsoftLock>().checkAll();
        }
        if (started && aliveEnemies == 0)
        {
            endCollider.gameObject.SetActive(false);
            startTrigger.gameObject.SetActive(false);
            started = false;
            ServiceLocator.Instance.Get<ILevelController>().endArea(this);
        }
    }
    public void destroyBottle(Throwable botel)
    {
        if (botel.gameObject.activeSelf)
        {
            intactBottles--;
            ServiceLocator.Instance.Get<IsoftLock>().checkAll();

        }
    }
    private void Start()
    {
        ServiceLocator.Instance.Get<ILevelController>().subscribeToRestart(restart);
    }

    void restart()
    {
        aliveEnemies = enemies.Count;
        started = false;
        foreach (AEnemyBehaviour enem in enemies)
        {
            print(enem.name);
            enem.gameObject.SetActive(true);
            enem.restart(this);
        }
        foreach (Throwable botel in botellas)
        {
            botel.gameObject.SetActive(true);
            botel.resTart(this);
        }
        print("restae");
        startTrigger.gameObject.SetActive(true);
        startCollider.gameObject.SetActive(false);
        endCollider.gameObject.SetActive(false);
    }
}
