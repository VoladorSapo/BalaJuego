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
        print("Softlock" + name);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            checkAll();
        }
    }
    public void checkAll()
    {
        restart_Text.gameObject.SetActive(false);
        numberAttack = numberEnemies = 0;
        if (currentArea)
        {
            foreach (var enemy in currentArea.enemies)
            {
                if (!enemy.life.dead)
                {
                    numberEnemies++;
                    if (enemy.GetComponentInChildren<IGun>() != null)
                    {
                        addAttack(enemy.GetComponentInChildren<IGun>().getBullets());

                    }
                    if (enemy.canBeKilledMelee)
                    {
                        addAttack(1);
                    }
                }
            }
            addAttack(currentArea.intactBottles);
            foreach (ABaseProyectile bul in FindObjectsOfType<ABaseProyectile>())
            {

                print(bul.name + bul.GetType());
                if (bul.GetType() != typeof(Throwable) && !bul.hit)
                {
                    addAttack(1);
                }
            }
            PlayerMove player = FindObjectOfType<PlayerMove>();
            if (player)
            {
                if (FindObjectOfType<PlayerMove>() != null)
                {
                    if (FindObjectOfType<PlayerMove>().gameObject.GetComponentInChildren<IGun>() != null)
                    {
                        addAttack(FindObjectOfType<PlayerMove>().gameObject.GetComponentInChildren<IGun>().getBullets());
                    }
                }
            }

            if (numberEnemies > numberAttack)
            {
                switch (settingManager.Instance.getLanguage())
                {
                    case Language.Spanish:
                        restart_Text.text = "Sin Balas Suficientes: Reinicia desde el Menú de Pausa";
                        break;
                    case Language.English:
                        restart_Text.text = "Not Enough Bullets: Restart from Pause Menu";

                        break;
                    case Language.Catalan:
                        restart_Text.text = "Sense Bales Suficients: Reinicia des del Menú de Pausa";

                        break;
                }
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
    void addAttack(int add)
    {
        if (add > 0)
        {
            print("addattack " +add);
        }
        numberAttack += add;
    }
}

public interface IsoftLock: IService
{
    public void checkAll();
}
