using UnityEngine;

public interface IEquipable
{
    public void Action(CharacterInteractor interactor, float angle);
    public GameObject getObj();
    public void setAsEquipment(Transform equipmentParent);

    public void Grab(CharacterInteractor interactor);

    public bool isFrontArmed();

    public RuntimeAnimatorController GetAnimator();

    public string getAnimatorLayerName();
    void setLayers(string layerName, int layerOrder);
    void resetLayers();
}
