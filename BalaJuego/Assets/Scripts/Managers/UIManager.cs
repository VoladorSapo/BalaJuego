using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
  [SerializeField]  CanvasGroup DeathScreen;
   [SerializeField] CanvasGroup PauseScreen;

    [SerializeField] CanvasGroup winScreen;

    // Start is called before the first frame update
    void Start()
    {
        ServiceLocator.Instance.Get<IGameState>().subscribeToStateChange(changeState);
        changeGroup(PauseScreen, false);
        changeGroup(winScreen, false);

        changeGroup(DeathScreen, false);
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
                changeGroup(PauseScreen, true);
                changeGroup(DeathScreen, false);
                changeGroup(winScreen, false);

                break;
            case IGameState.gameState.Death:
                changeGroup(PauseScreen, false);
                changeGroup(DeathScreen, true);
                            changeGroup(winScreen, false);

                break;
            case IGameState.gameState.Win:
                changeGroup(PauseScreen, false);
                changeGroup(DeathScreen, false);
                changeGroup(winScreen, true);
                break;
            default:
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

        changeGroup(PauseScreen, false);
        changeGroup(DeathScreen, false);
        changeGroup(winScreen, false);
        ServiceLocator.Instance.Get<ILevelController>().reStart();
    }
}

