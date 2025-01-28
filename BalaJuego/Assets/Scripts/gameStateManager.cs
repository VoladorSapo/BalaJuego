using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameStateManager : MonoBehaviour, IGameState
{
    public event EventHandler<stateData> onTimeChange;

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
        state = IGameState.gameState.NormalTime;
    }
    public IGameState.gameState getState() => state;
    public IGameState.gameState state;
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
    void setState(IGameState.gameState newState)
    {
        stateData data = new stateData(state, newState);

        state = newState;
        onTimeChange?.Invoke(this, data);
    }
}
