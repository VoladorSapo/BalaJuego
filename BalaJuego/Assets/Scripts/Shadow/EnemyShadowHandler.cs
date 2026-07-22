using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShadowHandler : MonoBehaviour
{
    [SerializeField] IShadowManger sM;
    [SerializeField] float shadowSize = 1;

    private void Start()
    {
        sM = ServiceLocator.Instance.Get<IShadowManger>();
    }
    void OnEnable()
    {
        sM.addEnemyTransform(new EnemyShadowInfo(this.transform, shadowSize));
    }

    void OnDisable()
    {
        sM.removeEnemyTransform(new EnemyShadowInfo(this.transform, shadowSize));
    }
}
