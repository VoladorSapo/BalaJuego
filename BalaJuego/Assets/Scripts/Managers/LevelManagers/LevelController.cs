using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour, ILevelController
{
    Action restartEvent;
    EventHandler<AreaData> endAreaEvent;
    EventHandler<LevelAreaController> startAreaEvent;

  [SerializeField]  List<LevelAreaController> areas;

    [SerializeField] GameObject levelAreaParent;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        areas = new List<LevelAreaController>();
        areas.AddRange(levelAreaParent.GetComponentsInChildren<LevelAreaController>());
        player = GameObject.FindObjectOfType<PlayerMove>().gameObject;
        StartCoroutine(lateStart());
        
    }
    IEnumerator lateStart()
    {
        yield return new WaitForEndOfFrame();
        reStart();
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
        AreaData data = new AreaData(area, areas[areas.IndexOf(area) +1]);
        endAreaEvent.Invoke(this, data);
    }
    public void startArea(LevelAreaController area)
    {
        startAreaEvent?.Invoke(this, area);
    }

    public void Win()
    {
        ServiceLocator.Instance.Get<IGameState>().setState(IGameState.gameState.Win);
    }

    public void Lose()
    {
        ServiceLocator.Instance.Get<IGameState>().setState(IGameState.gameState.Death);

    }

    public void reStart()
    {
        restartEvent.Invoke();
    }

    public void Instantiate()
    {

    }
}
