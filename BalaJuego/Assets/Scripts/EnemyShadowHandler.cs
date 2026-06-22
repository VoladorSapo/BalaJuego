using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShadowHandler : MonoBehaviour
{
    [SerializeField] ShadowManager sM;

    void OnEnable()
    {
        sM.EnemyTransforms.Add(this.transform);
    }

    void OnDisable()
    {
        sM.EnemyTransforms.Remove(this.transform);
    }
}
