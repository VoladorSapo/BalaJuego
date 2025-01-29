using System;

public interface IGameState : IService
{
    public enum gameState
    {
        Paused,
        NormalTime,
        Cinematic,
        SlowDown,
        Death,
        Win
    }
    public gameState getState();

    public void setState(gameState newState);

    public void Pause();
    public void UnPause();


    public void subscribeToStateChange(EventHandler<stateData> response);

    public void unSubscribeToStateChange(EventHandler<stateData> response);


}