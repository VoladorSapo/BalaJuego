using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timecounter : MonoBehaviour
{
    bool runningTime = false;
    [SerializeField] float TimePassed;
    public static timecounter Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ServiceLocator.Instance.Get<IGameState>().subscribeToStateChange(changeState);

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
}
