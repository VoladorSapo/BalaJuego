using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HitStopManager : AHitstopBase, IHitStop
{
    protected override void Stop()
    {
        Time.timeScale = 0;

    }
    protected override void Continue()
    {
        Time.timeScale = 1;

    }
}
public interface IHitStop : IService
{
    public void HitStop(float duration, float delay = 0);
}
