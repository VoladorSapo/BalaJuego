using System;

public interface ITimeManager : IService
{
    public void changeTimeMagnitude(float newMagnitude,bool inf = false);

    public void subscribeToTimeChange(EventHandler<timeData> response);

    public void unSubscribeToTimeChange(EventHandler<timeData> response);

    public float getMagnitude();

}
