using System;
using tutorial;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class cutsceneCaller:MonoBehaviour
{
    [SerializeField] public postCutsceneAction actionType;

    [SerializeField] public Tutorial _tutorial;

  [SerializeField]  string nextScene;

   public PlayableAsset timeline;

    [SerializeField] bool playOnAwakeNoLevel = false;

    private void Start()
    {
        if (playOnAwakeNoLevel)
        {
            PlayCutscene();
        }
    }

    public void PlayCutscene()
    {
        Action _action = () => { };
        switch (actionType)
        {
            case postCutsceneAction.Tutorial:
                print("preparaTutorial");
                _action = () => { _tutorial.startTutorial(); };
                break;
            case postCutsceneAction.StartLevel:
                _action = () => { ServiceLocator.Instance.Get<ILevelController>().reStart(); };
                break;
            case postCutsceneAction.Continue:
                _action = () => { ServiceLocator.Instance.Get<IGameState>().setState(IGameState.gameState.NormalTime); };
                break;
            case postCutsceneAction.changeScene:
                _action = () => { SceneManager.LoadScene(nextScene); };
                break;
            default:
                break;
        }
        ServiceLocator.Instance.Get<IcutsceneManager>().startCutscene(timeline, _action);
    }

    public enum postCutsceneAction
    {
        Tutorial,
        StartLevel,
        Continue,
        changeScene
    }
}
