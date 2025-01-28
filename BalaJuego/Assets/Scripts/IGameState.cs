public interface IGameState : IService
{
    public enum gameState
    {
        Paused,
        NormalTime,
        Cinematic,
        SlowDown
    }
    public gameState getState();
}