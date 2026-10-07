using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterInteractor:MonoBehaviour, IDetectable
{
    [field:SerializeField] public Transform equipmentParent { get; private set; }

    protected UnityEvent<characterGunChangeData> playerGunChangeEvent;
    [SerializeField] protected GameObject currentEquipment;

    public void changePlayerGun(bool _hasAnything, IEquipable _equipment = null)
    {
        print("changeplayergun" + _hasAnything);
        playerGunChangeEvent.Invoke(new characterGunChangeData(_hasAnything, _equipment));
    }
    public void subscribeToPlayerGunChange(UnityAction<characterGunChangeData> response)
    {
        playerGunChangeEvent?.AddListener(response);

    }
    public void unSubscribeToPlayerGunChange(UnityAction<characterGunChangeData> response)
    {
        playerGunChangeEvent?.RemoveListener(response);

    }

    public virtual void getEquipment(GameObject interactableObj)
    {
        print(interactableObj);
        IEquipable equipable = interactableObj.GetComponent<IEquipable>();
        changePlayerGun(true, equipable);
        currentEquipment = interactableObj;
    }

    internal void loseEquipment()
    {
        currentEquipment.transform.parent = null;
        currentEquipment = null;
    }

    public GameObject getObj() => gameObject;
}