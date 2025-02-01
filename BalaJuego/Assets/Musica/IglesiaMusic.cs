using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IglesiaMusic : MonoBehaviour
{
    void Start()
    { 
        musicManager.Instance.SetPhase(1);
    }
    void Awake()
    { 
        //musicManager.Instance.SetPhase(1);
    }
}