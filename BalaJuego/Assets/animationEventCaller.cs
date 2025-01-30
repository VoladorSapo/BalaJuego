using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationEventCaller : MonoBehaviour
{
   public void endChrageHeavyEvent()
    {
        GetComponentInParent<HeavyEnemyController>().finishCharging = true;
    }
}
