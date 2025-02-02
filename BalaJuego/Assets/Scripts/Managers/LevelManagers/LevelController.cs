using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour, ILevelController
{

 [SerializeField]   LevelAreaController currentArea;
    Action restartEvent;
    EventHandler<AreaData> endAreaEvent;
    EventHandler<LevelAreaController> startAreaEvent;

  [SerializeField]  List<LevelAreaController> areas;

    [SerializeField] GameObject levelAreaParent;

    GameObject player;

  [SerializeField]  cutsceneCaller cutsceneStart;
    [SerializeField] cutsceneCaller cutsceneEnd;

    // Start is called before the first frame update
    void Start()
    {
        areas = new List<LevelAreaController>();
        areas.AddRange(levelAreaParent.GetComponentsInChildren<LevelAreaController>());
        player = GameObject.FindObjectOfType<PlayerMove>().gameObject;
        trueStart();
     
    }
    public void trueStart()
    {
        cutsceneStart.PlayCutscene();
        //StartCoroutine(lateStart());

        // AQUI MUSICA
        if ((SceneManager.GetActiveScene().name == "nivel1") && (SceneManager.GetActiveScene().name == "nivel1"))
        {
            musicManager.Instance.SetSong("nivel");
            musicManager.Instance.SetPhase(0);
        }
        else
        {
            musicManager.Instance.MuteSong();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            startArea(areas[0]);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            endArea(areas[0]);
        }

    }
    public void subscribeToRestart(Action response)
    {
        restartEvent += response;

    }
    public void unSubscribeToRestart(Action response)
    {
        restartEvent -= response;
    }


    public void subscribeToAreaEnd(EventHandler<AreaData> response)
    {
        endAreaEvent += response;

    }
    public void unSubscribeToAreaEnd(EventHandler<AreaData> response)
    {
        endAreaEvent -= response;
    }


    public void subscribeToAreaStart(EventHandler<LevelAreaController> response)
    {
        startAreaEvent += response;

    }
    public void unSubscribeToAreaStart(EventHandler<LevelAreaController> response)
    {
        startAreaEvent -= response;
    }

 


    public void endArea(LevelAreaController area)
    {
        currentArea = null;

        AreaData data = new AreaData(area, areas[areas.IndexOf(area) +1]);
        endAreaEvent.Invoke(this, data);
    }
    public void startArea(LevelAreaController area)
    {
        currentArea = area;
        startAreaEvent?.Invoke(this, area);
    }

    public void Win()
    {
        cutsceneEnd.PlayCutscene();
      // ServiceLocator.Instance.Get<IGameState>().setState(IGameState.gameState.Win);
    }

    public void Lose()
    {
        ServiceLocator.Instance.Get<IGameState>().setState(IGameState.gameState.Death);

    }

    public void reStart()
    {
        foreach (var item in FindObjectsOfType<baseBullet>())
        {
            if(item.GetComponent<botella>() == null)
            {
                Destroy(item.gameObject);
            }
        }
        restartEvent.Invoke();
    }

    public void Instantiate()
    {

    }

    public void playLastCutscene()
    {
        cutsceneEnd.PlayCutscene();
    }
}
