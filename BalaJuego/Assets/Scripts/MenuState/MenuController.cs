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
        FindObjectOfType<TimerManager>().Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown("1"))
            {
                callDithering("nivel1");
            }

         else if (Input.GetKeyDown("2"))
            {
                callDithering("nivel2");
            }

         else   if (Input.GetKeyDown("3"))
            {
                callDithering("nivel3");
            }
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
