using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CreditsMenuState : AMenuState
{
    Animator[] animators;
    public CreditsMenuState(MenuController menu) : base(menu)
    {
    }

    public override void FixedUpdate()
    {
    }

    public override void HandleButton(string i)
    {
        switch(i)
        {
            case "exit":
                menu.SetState(new MainMenuState(menu));
                break;
        }
    }

    public override void OnEnter()
    {
        menu.CreditsStateParent.SetActive(true);
        animators = menu.CreditsStateParent.GetComponentsInChildren<Animator>();
        foreach (Animator an in animators)
        {
            an.Play("moveIn");
        }
    }

    public override void OnExit()
    {
        foreach (Animator an in animators)
        {
            an.Play("moveOut");
        }
    }

    public override void Update()
    {
    }
}
