using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class OptionsMenuState : AMenuState
{
    public OptionsMenuState(MenuController menu) : base(menu)
    {
    }

    public override void FixedUpdate()
    {
    }

    public override void HandleButton(string i)
    {
     
    }

    public override void OnEnter()
    {
        menu.MainStateParent.SetActive(true);
    }

    public override void OnExit()
    {
        menu.MainStateParent.SetActive(false);
    }

    public override void Update()
    {
    }
}
