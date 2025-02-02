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

}
