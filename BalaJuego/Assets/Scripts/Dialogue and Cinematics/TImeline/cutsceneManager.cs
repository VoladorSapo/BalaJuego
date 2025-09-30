using System;
using UnityEngine;
using UnityEngine.Playables;

public class cutsceneManager : MonoBehaviour,IcutsceneManager{

    Action endCutsceneAction;
    [SerializeField] PlayableDirector director;
    CutsceneData currentData;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if(currentData != null && currentData.canBeSkipped == true)
           skipCutscene();
        }
    }
    public void endAnimation()
    {
        print("helou");
        print(endCutsceneAction.ToString());
        endCutsceneAction.Invoke();
    }

    public void Instantiate()
    {
        currentData = null; 
    }

    public void startCutscene(PlayableAsset timeline, Action endAction,CutsceneData data)
    {
        ServiceLocator.Instance.Get<IGameState>().setState(IGameState.gameState.Cinematic);
        currentData = data;
        director.playableAsset = timeline;
        endCutsceneAction = endAction;
        if (!settingManager.Instance.modeSpeedRun || !currentData.canBeSkipped)
        {
            director.time = 0;
            director.Play();
        }
        else
        {
            print("SALTANDO CINEMATICA");
            director.RebuildGraph(); // the graph must be created before getting the playable graph
            director.playableGraph.GetRootPlayable(0).SetSpeed(9999999);
            director.Play();
        }
    }
    public void skipCutscene()
    {
        print("SKIP");
        //if (currentData.objectsToTurnOff != null && currentData.objectsToTurnOff.Length > 0)
        //{
        //    foreach (var item in currentData.objectsToTurnOff)
        //    {
        //        item.SetActive(false);
        //    }
        //}
        director.RebuildGraph(); // the graph must be created before getting the playable graph
        director.playableGraph.GetRootPlayable(0).SetSpeed(9999999);
        director.Play();
    }

    public void PlaySound(string sound)
    {
        musicManager.Instance.PlaySoundPitch(sound);
    }
}

public interface IcutsceneManager : IService
{
    public void startCutscene(PlayableAsset timeline, Action endAction, CutsceneData data);
    public void skipCutscene();
}
public class CutsceneData{
 public   GameObject[] objectsToTurnOff;
    public bool canBeSkipped;
    public CutsceneData(bool _canSkipped,GameObject[] _objectsOff = null)
    {
        objectsToTurnOff = _objectsOff;
        canBeSkipped = _canSkipped;
    } 
}