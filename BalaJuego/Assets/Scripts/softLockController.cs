using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class softLockController : MonoBehaviour,IsoftLock
{
    [SerializeField] TMP_Text restart_Text;
    LevelAreaController currentArea;

  public  int numberAttack;
  public  int numberEnemies;
    // Start is called before the first frame update
    void Start()
    {
        ServiceLocator.Instance.Get<ILevelController>().subscribeToAreaStart(startArea);
        ServiceLocator.Instance.Get<ILevelController>().subscribeToRestart(restart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void checkAll()
    {
        restart_Text.gameObject.SetActive(false);
        numberAttack = numberEnemies = 0;
        if (currentArea)
        {
            foreach (var enemy in currentArea.enemies)
            {
                numberEnemies++;
                if (enemy.GetComponent<IShoot>() != null)
                {
                    numberAttack += enemy.GetComponent<IShoot>().getBullets();
                }
                if (enemy.canBeKilledMelee)
                {
                    numberAttack++;
                }
            }
            numberAttack += currentArea.intactBottles;
            foreach (baseBullet bul in FindObjectsOfType<baseBullet>())
            {
                if (!bul.hit)
                {
                    numberAttack++;
                }
            }
            PlayerMove player = FindObjectOfType<PlayerMove>();
            if (player)
            {
                numberAttack += FindObjectOfType<PlayerMove>().gameObject.GetComponentInChildren<IShoot>().getBullets();
            }

            if (numberEnemies > numberAttack)
            {
                restart_Text.gameObject.SetActive(true);

            }
        }
    }
    void restart()
    {
        print("restart");
        currentArea = null; 
        restart_Text.gameObject.SetActive(false);
    }
    void startArea(object sender, LevelAreaController data)
    {

        currentArea = data;

    }

    public void Instantiate()
    {

    }
}

public interface IsoftLock: IService
{
    public void checkAll();
}
