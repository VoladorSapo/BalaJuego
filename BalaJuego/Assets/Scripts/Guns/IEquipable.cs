using UnityEngine;

public interface IEquipable
{
    public void Action(ACharacterLife shooter, float angle);
    public GameObject getObj();
    public void setAsEquipment(Transform equipmentParent);

}
