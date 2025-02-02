using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuState : AMenuState
{
    Animator[] animators;
    public MainMenuState(MenuController menu) : base(menu)
    {
    }

    public override void FixedUpdate()
    {
    }

    public override void HandleButton(string i)
    {
        switch(i)
        {
            case "play":
                SceneManager.LoadScene("iglesia");
                break;
            case "exit":
                Application.Quit();
                break;
            case "credits":
                menu.SetState(new CreditsMenuState(menu));
                break;
            case "options":
                menu.SetState(new OptionsMenuState(menu));
                break;
        }
    }

    public override void OnEnter()
    {
        menu.MainStateParent.SetActive(true);
        animators = menu.MainStateParent.GetComponentsInChildren<Animator>();
        foreach(Animator an in animators)
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
