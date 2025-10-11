using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using TMPro;
public class cutsceneManager : MonoBehaviour,IcutsceneManager{

    Action endCutsceneAction;
    [SerializeField] PlayableDirector director;
    CutsceneData currentData;
    public bool isSkipingCutscene;
    public bool cutscenPlaying;
    [SerializeField] float currrentSkipPressTime;
    [SerializeField] float SkipPressTime;
   [SerializeField] Image skipCupstecenesBar;
    [SerializeField] TMP_Text textoInstruccionSaltar;

  [SerializeField]  int StateeFade = 0;
  [SerializeField]  float alpha = 0;


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
        if(currentData != null && currentData.canBeSkipped == true && cutscenPlaying && !isSkipingCutscene)
        {
            if (Input.anyKey || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                StateeFade = 1;
            }
            else
            {
                StartCoroutine(waitTurnOFfSkipAdvice());
            }
        }
        switch (StateeFade)
        {
            case 0:

                break;
            case 1:
                 alpha = textoInstruccionSaltar.color.a + (Time.deltaTime);
                if(alpha > 1)
                {
                    alpha = 1;
                    StateeFade = 0;

                }
                textoInstruccionSaltar.color = new Color(1, 1, 1, alpha);

                break;

                case 2:

                alpha = textoInstruccionSaltar.color.a - (Time.deltaTime);
                if (alpha < 0)
                {
                    alpha = 0;
                    StateeFade = 0;
                }
                textoInstruccionSaltar.color = new Color(1, 1, 1, alpha);

                break;
        }
    }
    IEnumerator waitTurnOFfSkipAdvice()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.5f / 10f);
            if (textoInstruccionSaltar.color.a == 0)
            {
                yield break;
            }
        }
        StateeFade = 2;
    }
    private void Start()
    {
        skipCupstecenesBar = GameObject.FindGameObjectWithTag("SkipUI").GetComponent<Image>();
        textoInstruccionSaltar = GameObject.FindGameObjectWithTag("SkipAdvice").GetComponent<TMP_Text>();
    }
    public void endAnimation()
    {
        StateeFade = 2;

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
            StateeFade = 2;
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
        StateeFade = 2;

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