using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public AMenuState currentState;
    [SerializeField] public GameObject MainStateParent;
    [SerializeField] public GameObject OptionStateParent;
    [SerializeField] public GameObject CreditsStateParent;
    // Start is called before the first frame update
    void Start()
    {
        SetState(new MainMenuState(this));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ISaveManager save = new SaveManager();
            string s = save.getSavedScene();
            if(s != null)
            callDithering(s);
        }
    }

    public void SetState(AMenuState state)
    {

        if (currentState != null)
        {
            currentState.OnExit();
        }

        currentState = state;
        currentState.OnEnter();
    }
    public void callDithering(string scene)
    {
        FindObjectOfType<ditherTransition>().goIn(scene);
    }
}
