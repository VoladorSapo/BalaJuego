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
        menu.currentState.HandleButton(name);
    }
}
