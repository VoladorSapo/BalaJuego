using UnityEngine;

public interface IEquipable
{
    public void Action(CharacterInteractor interactor, float angle);
    public GameObject getObj();
    public void setAsEquipment(Transform equipmentParent);

    public void Grab(CharacterInteractor interactor);


}
