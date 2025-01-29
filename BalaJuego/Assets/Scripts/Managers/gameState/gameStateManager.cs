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
       if(data.currentMagnitude == 1){
            setState(IGameState.gameState.NormalTime);
        }
        else
        {
            setState(IGameState.gameState.SlowDown);

        }
    }
  public  void setState(IGameState.gameState newState)
    {
        stateData data = new stateData(currentState, newState);

        currentState = newState;
        onStateChange?.Invoke(this, data);
    }


    public void subscribeToTimeChange(EventHandler<stateData> response)
    {
        onStateChange += response;

    }

    public void unSubscribeToTimeChange(EventHandler<stateData> response)
    {
        onStateChange -= response;
    }
  
    public void Pause()
    {
        prePauseState = currentState;
        setState(IGameState.gameState.Paused);
        Time.timeScale = 0;
    }

    public void UnPause()
    {
      
            setState(prePauseState);
        Time.timeScale = 1;

    }

    public void Die()
    {
        setState(IGameState.gameState.Death);
    }
}
