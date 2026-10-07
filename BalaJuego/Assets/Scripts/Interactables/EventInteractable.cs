using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// La gran mayoría de elementos interactuables utilizan esta clase
/// Permite usar eventos de Unity para que las acciones de interactuar, hover y demás se establezcan en el editor.
/// Evitando tener que crear una clase específica por objeto
/// </summary>
public class EventInteractable : MonoBehaviour, IInteractable
{
    public UnityEvent<PlayerInteractor> interactEvent;
    public UnityEvent<bool> setOnRadiusEvent;
    public UnityEvent mouseEnter;
    public UnityEvent mouseLeave;
    public UnityEvent<bool> onTimeStop;


    bool isSelect;
   [SerializeField] private bool canInteract;

    public GameObject getObj() => gameObject;

    public bool inSelect() => isSelect;

    public void interact(PlayerInteractor player) => interactEvent.Invoke(player);
    public void setOnRadius(bool set)
    {
        if(isSelect && !set)
        {
            print(gameObject.name);
            mouseLeave?.Invoke();
        }
        isSelect = set;
        setOnRadiusEvent.Invoke(set);
    }

    private void Start()
    {
        isSelect = false;
        ServiceLocator.Instance.Get<IGameState>().subscribeToStateChange(onStateChange);
    }

    private void onStateChange(object sender, stateData e)
    {
        switch (e.currentState)
        {
            case IGameState.gameState.SlowDown:
                onTimeStop?.Invoke(true);
                break;
            case IGameState.gameState.NormalTime:
            case IGameState.gameState.Tutorial:
                onTimeStop?.Invoke(false);
                break;
        }
    }

    private void OnMouseOver()
    {
        if (inSelect())
        {
            print("mouseEnter");
            mouseEnter?.Invoke();
        }
    }
    private void OnMouseExit()
    {
        print("mouseExit");
        mouseLeave?.Invoke();
    }

    public bool CanInteract() => canInteract;

    public void setCanInteract(bool set) => canInteract=set;
}