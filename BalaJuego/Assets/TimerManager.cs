using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class TimerManager : MonoBehaviour, ITimer
{
    bool runningTime = false;
    [SerializeField] float TimePassed;
    static TimerManager Instance;  

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void changeState(object sender, stateData e)
    {
        switch (e.currentState)
        {
            case IGameState.gameState.NormalTime:
                runningTime = true;


                break;
            case IGameState.gameState.SlowDown:
                runningTime = true;

                break;
            //case IGameState.gameState.Death:
            //    runningTime = true;

            //    break;
            default:
                runningTime = false;

                break;


        }
    }

    // Update is called once per frame
    void Update()
    {
        if (runningTime)
        {
            TimePassed += Time.deltaTime;
        }
    }

    public void Instantiate()
    {
        ServiceLocator.Instance.Get<IGameState>().subscribeToStateChange(changeState);
    }

    public TimePoints getSeconds()
    {
        return new TimePoints(TimePassed);
    }

    public void Reset()
    {
        TimePassed = 0;
    }
}
public interface ITimer : IService
{
    public TimePoints getSeconds();
}
public struct TimePoints
{
 public   int hours, minutes, seconds;

   public TimePoints(float allSeconds)
    {
        hours = (int)MathF.Floor(allSeconds / 3600);
        minutes = (int)MathF.Floor((allSeconds - hours * 3660) / 60);
        seconds = (int)MathF.Floor((allSeconds - hours * 3600) % 60);
    }
    public override string ToString()
    {
        return $"H: {hours} M: {minutes} S: {seconds}";
    }
}
