using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterInteractor:MonoBehaviour, IDetectable
{
    public IGun shoot { get; protected set; }

    [field:SerializeField] public Transform backequipmentParent { get; private set; }
    [field: SerializeField] public Transform frontequipmentParent { get; private set; }


    protected UnityEvent<characterGunChangeData> playerGunChangeEvent;
    [SerializeField] protected GameObject currentEquipment;

    [SerializeField] protected gunRotate gunRotate;
    AnimatorLayerManager animatorLayerManager;
    public void changePlayerGun(bool _hasAnything, IEquipable _equipment = null)
    {
        var anim = GetComponentsInChildren<Animator>(true)[1].GetComponent<AnimatorLayerManager>();
        if (anim != null)
        {
            if (!_hasAnything) anim.ResetPickfromLayers();
        }

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
        gunRotate.frontArmOn = equipable.isFrontArmed();
        animatorLayerManager.SwapAnim(equipable.GetAnimator());
        animatorLayerManager.PickLayer(equipable.getAnimatorLayerName());
    }

    internal void loseEquipment()
    {
        currentEquipment.transform.parent = null;
        currentEquipment = null;
    }

    public GameObject getObj() => gameObject;

    protected virtual void Awake()
    {
        animatorLayerManager = GetComponentInChildren<AnimatorLayerManager>();
        gunRotate = GetComponentInChildren<gunRotate>();
    }
}