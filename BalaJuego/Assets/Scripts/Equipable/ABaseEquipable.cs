using UnityEngine;

public abstract class ABaseEquipable:MonoBehaviour,IEquipable
{
    [SerializeField] LevelAreaController area;

    [SerializeField] Vector3 initialPos;

    [SerializeField] Vector3 positionWhenEquipped;
    [SerializeField] Vector3 rotationWhenEquipped;

    [SerializeField] RuntimeAnimatorController animatorForAgatha;
    [SerializeField] bool frontArmed;

    public CharacterInteractor owner { get;private set; }

    public virtual void Grab(CharacterInteractor character)
    {
        GetComponentInChildren<Animator>().Play("onGrab");
        musicManager.Instance.PlaySoundPitch("snd_pick", 0.2f);
        character.getEquipment(gameObject);
        setAsEquipment(frontArmed ? character.frontequipmentParent : character.backequipmentParent);
        owner = character;
    }

    public virtual void setAsEquipment(Transform equipmentParent)
    {
        transform.parent = equipmentParent;
        transform.localPosition = positionWhenEquipped;
        transform.localEulerAngles = rotationWhenEquipped;
    }

    public GameObject getObj() => gameObject;
    public abstract void Action(CharacterInteractor interactor, float angle);

    public void DestroyEquipment()
    {
        transform.parent = null;
        owner.loseEquipment();
        Destroy(gameObject);
    }

    public void throwOut()
    {
        IProyectile proyectile = GetComponent<IProyectile>();
        proyectile.DeactivateMovement();
    }

    public bool isFrontArmed() => frontArmed;
    public RuntimeAnimatorController GetAnimator() => animatorForAgatha;
}