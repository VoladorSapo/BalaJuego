public class stateData
{
    IGameState.gameState oldState;
    IGameState.gameState currentState;

    public stateData(IGameState.gameState _old, IGameState.gameState _new)
    {
        oldState = _old;
        currentState = _new;
    }

            

}