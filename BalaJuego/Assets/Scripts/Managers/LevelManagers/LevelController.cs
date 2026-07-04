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


    [SerializeField] bool isChurch = false;
    // Start is called before the first frame update
    void Start()
    {
        if (levelAreaParent != null)
        {
            areas = new List<LevelAreaController>();
            areas.AddRange(levelAreaParent.GetComponentsInChildren<LevelAreaController>());

            PlayerMove move = FindObjectOfType<PlayerMove>();
            if (move)
            {
                player = GameObject.FindObjectOfType<PlayerMove>().gameObject;
            }
        }
    }
    public void trueStart()
    {
        if (cutsceneStart != null)
        {
            cutsceneStart.PlayCutscene();
            //StartCoroutine(lateStart());

            // AQUI MUSICA
            if (SceneManager.GetActiveScene().name == "nivel1") //|| (SceneManager.GetActiveScene().name == "nivel2"))
            {
                musicManager.Instance.SetSong("nivel");
                musicManager.Instance.SetPhase(0);
            }
            else if (SceneManager.GetActiveScene().name == "nivel3")
            {
                if (!isChurch)
                {
                    musicManager.Instance.FadeOutCurrentSong();
                    //musicManager.Instance.MuteSong();
                }
            }
        }
        else
        {
            print("restart");
            reStart();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            restartEvent.Invoke();
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
        foreach (var item in FindObjectsOfType<ABaseProyectile>())
        {
            if(item.GetComponent<Throwable>() == null)
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
