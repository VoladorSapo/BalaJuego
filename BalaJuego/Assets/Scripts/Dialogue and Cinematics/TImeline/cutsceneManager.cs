using System;
using UnityEngine;
using UnityEngine.Playables;

public class cutsceneManager : MonoBehaviour,IcutsceneManager{

    Action endCutsceneAction;
    [SerializeField] PlayableDirector director;
    
    public void endAnimation()
    {
        print("helou");
        print(endCutsceneAction.ToString());
        endCutsceneAction.Invoke();
    }

    public void Instantiate()
    {

    }

    public void startCutscene(PlayableAsset timeline, Action endAction)
    {
        ServiceLocator.Instance.Get<IGameState>().setState(IGameState.gameState.Cinematic);

        director.playableAsset = timeline;
        endCutsceneAction = endAction;
        director.time = 0;
        director.Play();
    }
}

public interface IcutsceneManager : IService
{
    public void startCutscene(PlayableAsset timeline, Action endAction);
}
