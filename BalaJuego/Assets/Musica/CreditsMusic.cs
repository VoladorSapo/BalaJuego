using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMusic : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true;
        musicManager.Instance.SetSong("credits");
        musicManager.Instance.SetPhase(0);
    }
    void Awake()
    { 
        //musicManager.Instance.SetPhase(1);
    }
}