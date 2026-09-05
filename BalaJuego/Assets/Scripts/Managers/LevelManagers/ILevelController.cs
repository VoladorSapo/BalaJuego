using System;
using UnityEngine;
public interface ILevelController: IService
{
    public void trueStart();
    public void endArea(LevelAreaController area);
    public void startArea(LevelAreaController area);

    public void Win();
    public void Lose();

    public void reStart();
    public void subscribeToRestart(Action response);
    public void unSubscribeToRestart(Action response);
    public void subscribeToAreaEnd(EventHandler<AreaData> response);


    public void unSubscribeToAreaEnd(EventHandler<AreaData> response);

    public void subscribeToAreaStart(EventHandler<LevelAreaController> response);


    public void unSubscribeToAreaStart(EventHandler<LevelAreaController> response);

    public void playLastCutscene();

    public GameObject getPlayer();

}
public class AreaData
{
    public LevelAreaController endArea;
    public LevelAreaController nextArea;

    public AreaData(LevelAreaController _end, LevelAreaController _next)
    {
        endArea = _end;
        nextArea = _next;
    }
}
