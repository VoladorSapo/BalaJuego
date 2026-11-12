using UnityEngine;

public abstract class ABaseEquipable : MonoBehaviour, IEquipable
{
    [SerializeField] LevelAreaController area;

    [SerializeField] Vector3 initialPos;

    [SerializeField] Vector3 positionWhenEquipped;
    [SerializeField] Vector3 rotationWhenEquipped;

    [SerializeField] RuntimeAnimatorController animatorForAgatha;
    [SerializeField] bool frontArmed;
    [SerializeField] string animatorLayerName;

    SpriteRenderer spriteRenderer;
    string defaultsortinglayer;
    int defaultsortingorder;

    public CharacterInteractor owner { get;private set; }
    protected virtual void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);
        if (spriteRenderer != null)
        {
            defaultsortinglayer = spriteRenderer.sortingLayerName;
            defaultsortingorder = spriteRenderer.sortingOrder;
        }
        DestroyRespawnController respawnController = GetComponent<DestroyRespawnController>();
        if (respawnController != null)
            respawnController.subscribeToRespawnAction(resetLayers);
    }
    public virtual void Grab(CharacterInteractor character)
    {
        GetComponentInChildren<Animator>().Play("onGrab");
        musicManager.Instance.PlaySoundPitch("snd_pick", 0.2f);
        setAsEquipment(frontArmed ? character.frontequipmentParent : character.backequipmentParent);
        character.getEquipment(gameObject);
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

    public string getAnimatorLayerName() => animatorLayerName;

    public void setLayers(string layerName, int layerOrder)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = layerName;
            spriteRenderer.sortingOrder = layerOrder;
        }
    }

    public void resetLayers()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = defaultsortinglayer;
            spriteRenderer.sortingOrder = defaultsortingorder;
        }
    }
}