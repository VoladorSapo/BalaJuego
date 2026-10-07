using UnityEngine;

public abstract class ABaseEquipable:MonoBehaviour,IEquipable
{
    [SerializeField] LevelAreaController area;

    [SerializeField] Vector3 initialPos;

    [SerializeField] Vector3 positionWhenEquipped;
    [SerializeField] Vector3 rotationWhenEquipped;

   public CharacterInteractor owner { get;private set; }

    protected void Start()
    {
        initialPos = transform.position;
    }
    public virtual void resTart(LevelAreaController _area)
    {

        area = _area;
        transform.position = initialPos;

    }

    public virtual void Grab(CharacterInteractor character)
    {
        musicManager.Instance.PlaySoundPitch("snd_pick", 0.2f);
        if (area)
        {
            area.destroyBottle(this);
        }
        character.getEquipment(gameObject);
        setAsEquipment(character.equipmentParent);
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
}