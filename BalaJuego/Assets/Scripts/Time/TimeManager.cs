using System;
using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour,ITimeManager
{
 [SerializeField]   private float timeMagnitude;
    [SerializeField] float timeLimit;

    public event EventHandler<timeData> onTimeChange;

    Coroutine waiting;

    bool hasChanged;

    void Start()
    {
        ServiceLocator.Instance.Get<IGameState>().subscribeToStateChange(changeState);
        ServiceLocator.Instance.Get<ILevelController>().subscribeToRestart(restart);

    }
    public void Instantiate()
    {
        timeMagnitude = 1;
    }

    public void changeTimeMagnitude(float newMagnitude, bool inf = false)
    {
        musicManager.Instance.changeTimeMagnitude(newMagnitude);

        Debug.Log("Change Time " + newMagnitude);
        float cacheMagnitude = timeMagnitude;
        timeMagnitude = newMagnitude;
        onTimeChange?.Invoke(this,new timeData(cacheMagnitude, newMagnitude));
        if (newMagnitude < 1 && !inf)
        {
            waiting = StartCoroutine(timeLimitReset());
        }
    }

    public void subscribeToTimeChange(EventHandler<timeData> response)
    {
        onTimeChange += response;
    }

    public void unSubscribeToTimeChange(EventHandler<timeData> response)
    {
        onTimeChange -= response;

    }

    public float getMagnitude() => timeMagnitude;

    public void restart()
    {
        changeTimeMagnitude(1);

    }
    public void changeState(object sender, stateData data)
    {
        switch (data.currentState)
        {
            case IGameState.gameState.Paused:
                
                break;
            case IGameState.gameState.NormalTime:
                if (waiting != null)
                {
                    StopCoroutine(waiting);
                }
                hasChanged = true;
                break;
            case IGameState.gameState.Cinematic:
                if (waiting != null)
                {
                    StopCoroutine(waiting);
                }
                hasChanged = true;
                break;
            case IGameState.gameState.SlowDown:
                if (waiting != null)
                {
                    StopCoroutine(waiting);
                }

                break;
            case IGameState.gameState.Death:
                if (waiting != null)
                {
                    StopCoroutine(waiting);
                }
                hasChanged = true;

                break;
            case IGameState.gameState.Win:
                if (waiting != null)
                {
                    StopCoroutine(waiting);
                }
                hasChanged = true;

                break;
            default:
                if (waiting != null)
                {
                    StopCoroutine(waiting);
                }
                hasChanged = true;

                break;
        }
    }
    IEnumerator timeLimitReset() 
    {
        hasChanged = false;
        int rounds = 10;
        for (int i = 0; i < rounds; i++)
        {
            yield return new WaitForSeconds(timeLimit / rounds);
            if(hasChanged)
            {
                yield break;
            }
        }
        if (!hasChanged)
        {
         //  changeTimeMagnitude(1);
        }
    }

    public void endSlow()
    {
        if (!hasChanged)
        {
           changeTimeMagnitude(1);
        }
    }
}
