using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour, ILevelController
{
    Action restartEvent;
    EventHandler<int> endAreaEvent;

  List<LevelAreaController> areas;

    [SerializeField] GameObject levelAreaParent;

    // Start is called before the first frame update
    void Start()
    {
        areas = new List<LevelAreaController>();
        areas.AddRange(levelAreaParent.GetComponentsInChildren<LevelAreaController>());
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void subscribeToRestart(Action response)
    {
        restartEvent += response;

    }

    public void unSubscribeToRestart(Action response)
    {
        restartEvent -= response;
    }
    public void subscribeToAreaEnd(EventHandler<int> response)
    {
        endAreaEvent += response;

    }

    public void unSubscribeToAreaEnd(EventHandler<int> response)
    {
        endAreaEvent -= response;
    }
    public void endArea(LevelAreaController area)
    {
        endAreaEvent.Invoke(this, areas.IndexOf(area));
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
