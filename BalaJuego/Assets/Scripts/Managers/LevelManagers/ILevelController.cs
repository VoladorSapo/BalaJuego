using System;

public interface ILevelController: IService
{
    public void endArea(LevelAreaController area);
    public void Win();
    public void Lose();

    public void reStart();
    public void subscribeToRestart(Action response);
    public void unSubscribeToRestart(Action response);
    public void subscribeToAreaEnd(EventHandler<int> response);


    public void unSubscribeToAreaEnd(EventHandler<int> response);

}
