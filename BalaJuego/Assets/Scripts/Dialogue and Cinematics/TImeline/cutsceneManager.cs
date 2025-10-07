using System;
using UnityEngine;
using UnityEngine.Playables;

public class cutsceneManager : MonoBehaviour,IcutsceneManager{

    Action endCutsceneAction;
    [SerializeField] PlayableDirector director;
    CutsceneData currentData;
    public bool isSkipingCutscene;
    public bool cutscenPlaying;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentData != null && currentData.canBeSkipped == true && cutscenPlaying && !isSkipingCutscene)
            {
                skipCutscene();
            }
            else
            {
                print("No se pue saltar");
            }
        }
       
    }
    public void endAnimation()
    {
        cutscenPlaying = false;
        isSkipingCutscene = false;
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
        if (!settingManager.Instance.skipCutscenes || !currentData.canBeSkipped)
        {
            isSkipingCutscene = false;
            director.time = 0;
            cutscenPlaying = true;
            director.Play();
        }
        else
        {
            cutscenPlaying = false;
            isSkipingCutscene = true;
            print("SALTANDO CINEMATICA");
            director.RebuildGraph(); // the graph must be created before getting the playable graph
            director.playableGraph.GetRootPlayable(0).SetSpeed(9999999);
            director.Play();
        }
    }
    public void skipCutscene()
    {
        print("SKIP");
        isSkipingCutscene = true;
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
        if (cutscenPlaying && !isSkipingCutscene)
        {
            musicManager.Instance.PlaySoundPitch(sound);
        }
    }
}

public interface IcutsceneManager : IService
{
    public void startCutscene(PlayableAsset timeline, Action endAction, CutsceneData data);
    public void skipCutscene();
    public void PlaySound(string sound);
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