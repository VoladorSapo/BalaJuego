using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class OptionsMenuState : AMenuState
{
    Animator[] animators;
    public OptionsMenuState(MenuController menu) : base(menu)
    {
    }

    public override void FixedUpdate()
    {
    }

    public override void HandleButton(string i)
    {
        switch (i)
        {
            case "exit":
                menu.SetState(new MainMenuState(menu));
                break;
        }
    }

    public override void OnEnter()
    {
        //menu.MainStateParent.SetActive(true);
        menu.OptionStateParent.SetActive(true);
        animators = menu.OptionStateParent.GetComponentsInChildren<Animator>();
        foreach (Animator an in animators)
        {
            an.Play("moveIn");
        }
    }

    public override void OnExit()
    {
        //menu.MainStateParent.SetActive(false);
        foreach (Animator an in animators)
        {
            an.Play("moveOut");
        }
    }

    public override void Update()
    {
    }
}
