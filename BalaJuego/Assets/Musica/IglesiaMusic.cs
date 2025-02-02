using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IglesiaMusic : MonoBehaviour
{
    void Start()
    { 
        musicManager.Instance.SetSong("iglesia");
        musicManager.Instance.SetPhase(0);
    }
    void Awake()
    { 
        //musicManager.Instance.SetPhase(1);
    }
}