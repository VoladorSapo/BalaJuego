using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerInput : MonoBehaviour
{

    [SerializeField] InputActionReference shootAction;
    [SerializeField] InputActionReference interactAction;
    [SerializeField] InputActionReference jumpAction;
    [SerializeField] InputActionReference meleeAction;
    [SerializeField] InputActionReference pauseAction;

    [SerializeField] InputActionReference stopTimeAction;

    [SerializeField] InputActionReference moveAction;


    public bool ShootDown { get; private set; }
    public bool ShootUp { get; private set; }

    public bool InteractDown { get; private set; }
    public bool InteractUp { get; private set; }
    public bool JumpDown { get; private set; }
    public bool JumpUp { get; private set; }

    public bool MeleeDown { get; private set; }
    public bool PauseDown { get; private set; }
    public bool StopTimeDown { get; private set; }
    public bool StopTimeUp { get; private set; }

    public float Move { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShootDown = shootAction.action.WasPressedThisFrame();
        ShootUp = shootAction.action.WasReleasedThisFrame();
        InteractDown = interactAction.action.WasPressedThisFrame();
        InteractUp = interactAction.action.WasReleasedThisFrame();
        JumpDown = jumpAction.action.WasPressedThisFrame();
        JumpUp = jumpAction.action.WasReleasedThisFrame();

        MeleeDown = meleeAction.action.WasPressedThisFrame();
        PauseDown = pauseAction.action.WasPressedThisFrame();
        StopTimeDown = stopTimeAction.action.WasPressedThisFrame();
        StopTimeUp = stopTimeAction.action.WasReleasedThisFrame();
        Move = moveAction.action.ReadValue<float>();
        print("updatePlayerInput" + moveAction.action.ReadValue<float>() +jumpAction.action.WasPressedThisFrame());

    }

}
