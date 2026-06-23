using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShadowHandler : MonoBehaviour
{
    [SerializeField] ShadowManager sM;
    [SerializeField] float shadowSize = 1;

    void OnEnable()
    {
        sM.EnemyTransforms.Add(new EnemyShadowInfo(this.transform, shadowSize));
    }

    void OnDisable()
    {
        sM.EnemyTransforms.Remove(new EnemyShadowInfo(this.transform, shadowSize));
    }
}
