using UnityEngine;

public interface IInteractable
{
    public void tryGrab(PlayerShoot player);

    public void setHover(bool set);

    public GameObject getObj();
}
