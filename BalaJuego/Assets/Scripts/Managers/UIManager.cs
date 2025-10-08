using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
  [SerializeField]  CanvasGroup DeathScreen;
   [SerializeField] CanvasGroup PauseScreen;

    [SerializeField] CanvasGroup winScreen;
    Animator deathAnim;

    [SerializeField] string menuName = "MainMenu";

    // Start is called before the first frame update
    void Start()
    {
        ServiceLocator.Instance.Get<IGameState>().subscribeToStateChange(changeState);
        changeGroup(PauseScreen, false);
        changeGroup(winScreen, false);

        changeGroup(DeathScreen, false);
        foreach(MenuLanguageText text in GetComponentsInChildren<MenuLanguageText>())
        {
            text.updateLanguage();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void changeState(object sender, stateData data)
    {
        switch (data.currentState)
        {
            case IGameState.gameState.Paused:
                musicManager.Instance.PlaySoundPitch("snd_menu");
                changeGroup(PauseScreen, true);
                changeGroup(DeathScreen, false);
                changeGroup(winScreen, false);

                break;
            case IGameState.gameState.Death:
                deathAnim = DeathScreen.gameObject.GetComponent<Animator>();
                deathAnim.Play("die", -1, 0);
                musicManager.Instance.PlaySound("snd_muerte");
                musicManager.Instance.StopWalking();
                musicManager.Instance.StopHeavyWalking();
                changeGroup(PauseScreen, false);
                changeGroup(DeathScreen, true);
                            changeGroup(winScreen, false);

                break;
            case IGameState.gameState.Win:
                musicManager.Instance.PlaySoundPitch("snd_aceptar");
                changeGroup(PauseScreen, false);
                changeGroup(DeathScreen, false);
                changeGroup(winScreen, false);
                break;
            default:
                //musicManager.Instance.PlaySoundPitch("snd_aceptar");
                changeGroup(PauseScreen, false);
                changeGroup(DeathScreen, false);
                changeGroup(winScreen, false);
                break;
        }
    }

    void changeGroup(CanvasGroup canvas, bool active)
    {
        canvas.alpha = active ? 1 : 0;
        canvas.blocksRaycasts = active;
    }

    public void unPause()
    {
        ServiceLocator.Instance.Get<IGameState>().UnPause();
    }

    public void Restart() {

        //musicManager.Instance.PlaySoundPitch("snd_aceptar");
        changeGroup(PauseScreen, false);
        changeGroup(DeathScreen, false);
        changeGroup(winScreen, false);
        ServiceLocator.Instance.Get<ILevelController>().reStart();
    }
    public void Menu()
    {
        ServiceLocator.Instance.Get<ISaveManager>().saveGame(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        FindObjectOfType<ditherTransition>().goIn(menuName);
    }
}

