using UnityEngine;
using UnityEngine.Events;

public interface IInteractable:IDetectable
{
    public void interact(PlayerInteractor player);

    public void setOnRadius(bool set);


    public bool inSelect();


    bool CanInteract();
    void setCanInteract(bool set);

}
