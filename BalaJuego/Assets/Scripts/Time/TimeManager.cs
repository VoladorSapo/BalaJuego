using System;
using UnityEngine;

public class TimeManager : ITimeManager
{
    private float timeMagnitude;

    public event EventHandler<timeData> onTimeChange;
    public void Instantiate()
    {
        timeMagnitude = 1;
    }

    public void changeTimeMagnitude(float newMagnitude)
    {
        Debug.Log("Change Time " + newMagnitude);
        float cacheMagnitude = timeMagnitude;
        timeMagnitude = newMagnitude;
        onTimeChange?.Invoke(this,new timeData(cacheMagnitude, newMagnitude));
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
}
