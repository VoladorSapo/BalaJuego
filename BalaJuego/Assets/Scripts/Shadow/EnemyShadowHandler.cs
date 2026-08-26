using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShadowHandler : MonoBehaviour
{
    [SerializeField] IShadowManger sM;
    [SerializeField] float shadowSize = 1;

    private void Awake()
    {
        print("ShadowAwake");
    }
    void OnEnable()
    {
        print("ShadowEnable");
        sM = ServiceLocator.Instance.Get<IShadowManger>();
        sM.addEnemyTransform(new EnemyShadowInfo(this.transform, shadowSize));
    }

    void OnDisable()
    {
        sM.removeEnemyTransform(new EnemyShadowInfo(this.transform, shadowSize));
    }
}
