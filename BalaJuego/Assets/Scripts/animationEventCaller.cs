using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationEventCaller : MonoBehaviour
{
   public void endChrageHeavyEvent()
    {
        GetComponentInParent<HeavyEnemyController>().finishCharging = true;
    }

    public void endReloadEvent()
    {
        GetComponentInParent<PlayerShoot>().endReloadAnim();

    }

    public void endMeleeAnim()
    {
        GetComponentInParent<PlayerShoot>().endMeleeAnim();

    }
    public void throwBottle()
    {
        GetComponentInParent<PlayerShoot>().throwBottle();

    }
    public void finishDeeathAnim()
    {
        GetComponentInParent<EnemyLife>().finishDeathAnim();

    }

}
