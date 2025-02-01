using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameStateManager : MonoBehaviour, IGameState
{
    public event EventHandler<stateData> onStateChange;
    public IGameState.gameState prePauseState;
    public IGameState.gameState getState() => currentState;
    public IGameState.gameState currentState;
    // Start is called before the first frame update
    void Start()
    {
        ServiceLocator.Instance.Get<ITimeManager>().subscribeToTimeChange(changeTimeMagnitude);
        ServiceLocator.Instance.Get<ILevelController>().subscribeToRestart(restart);

    }


    // Update is called once per frame
    void Update()
    {

    }

    public void Instantiate()
    {
        currentState = IGameState.gameState.NormalTime;
    }
   
    void changeTimeMagnitude(object sender, timeData data)
    {
        if (currentState != IGameState.gameState.Tutorial)
        {
            if (data.currentMagnitude == 1)
            {
                setState(IGameState.gameState.NormalTime);
            }
            else
            {
                setState(IGameState.gameState.SlowDown);

            }
        }
    }
  public  void setState(IGameState.gameState newState)
    {
        stateData data = new stateData(currentState, newState);

        currentState = newState;
        switch (newState)
        {
            case IGameState.gameState.Paused:
                Time.timeScale = 0;
                break;
            default:
                Time.timeScale = 1;

                break;
        }
        onStateChange?.Invoke(this, data);
        
    }


    public void subscribeToStateChange(EventHandler<stateData> response)
    {
        onStateChange += response;

    }

    public void unSubscribeToStateChange(EventHandler<stateData> response)
    {
        onStateChange -= response;
    }
  
    public void Pause()
    {
        prePauseState = currentState;
        setState(IGameState.gameState.Paused);
    }

    public void UnPause()
    {
      
            setState(prePauseState);

    }

   
    public void restart()
    {
        setState(IGameState.gameState.NormalTime);
        print("setState");

    }
}
