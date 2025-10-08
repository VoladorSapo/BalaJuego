using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class cutsceneManager : MonoBehaviour,IcutsceneManager{

    Action endCutsceneAction;
    [SerializeField] PlayableDirector director;
    CutsceneData currentData;
    public bool isSkipingCutscene;
    public bool cutscenPlaying;
    [SerializeField] float currrentSkipPressTime;
    [SerializeField] float SkipPressTime;
   [SerializeField] Image skipCupstecenesBar;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (currentData != null && currentData.canBeSkipped == true && cutscenPlaying && !isSkipingCutscene)
            {
                currrentSkipPressTime += Time.deltaTime;
                if (currrentSkipPressTime >= SkipPressTime)
                {
                    skipCutscene();
                }
            }
            else
            {
                print("No se pue saltar");
            }
        }
        else
        {
            currrentSkipPressTime -= Time.deltaTime;
            if(currrentSkipPressTime < 0)
            {
                currrentSkipPressTime = 0;
            }
        }
        skipCupstecenesBar.fillAmount = currrentSkipPressTime / SkipPressTime;

    }
    private void Start()
    {
        skipCupstecenesBar = GameObject.FindGameObjectWithTag("SkipUI").GetComponent<Image>();
    }
    public void endAnimation()
    {
        skipCupstecenesBar.enabled = false;
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
            skipCupstecenesBar.enabled = true;

        }
        else
        {
            cutscenPlaying = false;
            isSkipingCutscene = true;
            print("SALTANDO CINEMATICA");
            if (!currentData.isEndLevel)
            {
            
                director.RebuildGraph(); // the graph must be created before getting the playable graph
                director.playableGraph.GetRootPlayable(0).SetSpeed(9999999);
                director.Play();
            }
            else
            {
                endAnimation();
            }
        }
    }
    public void skipCutscene()
    {
        print("SKIP");
        isSkipingCutscene = true;
        skipCupstecenesBar.enabled = false;

        //if (currentData.objectsToTurnOff != null && currentData.objectsToTurnOff.Length > 0)
        //{
        //    foreach (var item in currentData.objectsToTurnOff)
        //    {
        //        item.SetActive(false);
        //    }
        if (!currentData.isEndLevel)
        {
            director.RebuildGraph(); // the graph must be created before getting the playable graph
            director.playableGraph.GetRootPlayable(0).SetSpeed(9999999);
            director.Play();
        }
        else
        {
            endAnimation();
        }
    }

    public void PlaySound(string sound)
    {
        if (cutscenPlaying && !isSkipingCutscene)
        {
            musicManager.Instance.PlaySoundPitch(sound);
        }
    }

    public bool isSkippingCutscene() => isSkipingCutscene;
}

public interface IcutsceneManager : IService
{
    public void startCutscene(PlayableAsset timeline, Action endAction, CutsceneData data);
    public void skipCutscene();
    public void PlaySound(string sound);
    public bool isSkippingCutscene();
}
public class CutsceneData{
 public   GameObject[] objectsToTurnOff;
    public bool canBeSkipped;
    public bool isEndLevel;
    public CutsceneData(bool _canSkipped,bool isEndLevel,GameObject[] _objectsOff = null)
    {
        objectsToTurnOff = _objectsOff;
        canBeSkipped = _canSkipped;
        this.isEndLevel = isEndLevel;
    } 
}