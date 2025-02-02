using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuButton : MonoBehaviour
{
    // Start is called before the first frame update
    MenuController menu;
    [SerializeField] string name;
    void Start()
    {
        menu = FindObjectOfType<MenuController>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        musicManager.Instance.PlaySoundPitch("snd_aceptar");


        if ((name == "play") || (name == "exit"))
        {
            musicManager.Instance.SetPhase(0);
        }
        else
        {
            musicManager.Instance.SetPhase(1);
        }

        menu.currentState.HandleButton(name);
    }
}
