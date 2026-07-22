using UnityEngine;

public class Throwable : ABaseProyectile, IEquipable
{
    public bool isThrown;
    [SerializeField] LevelAreaController area;
    [SerializeField] Vector3 initialPos;

    [SerializeField] Vector3 positionWhenEquipped;
    [SerializeField] Vector3 rotationWhenEquipped;

    protected override void Start()
    {
        base.Start();
        initialPos = transform.position;
    }
    public void resTart(LevelAreaController _area)
    {

        area = _area;
        transform.position = initialPos;

    }
    public override void setHover(bool set)
    {
        if (grabable)
        {
            base.setHover(set);
            musicManager.Instance.PlaySoundPitch("bip", 0.2f);
        }
    }
    public override void ActivateProyectileMovement(ACharacterLife shooter, float angle)
    {
        transform.parent = null;
        musicManager.Instance.PlaySoundPitch("snd_lanzabotella");
        print(shooter.transform.localScale.x);
        angle *= shooter.transform.localScale.x;
        GetComponent<Collider2D>().enabled = true;
        transform.eulerAngles = new Vector3(0, shooter.transform.localScale.x < 0 ? -180 : 0, angle);
        this.owner = shooter;
        //anim = GetComponentInChildren<Animator>(true);
        //if (anim != null)
        //    anim.Play("bottleFly");
        moving = true;
    }

    public override void tryGrab(PlayerShoot player)
    {
        //if (area)
        //{
        musicManager.Instance.PlaySoundPitch("snd_pick", 0.2f);
        ServiceLocator.Instance.Get<ITimeManager>().changeTimeMagnitude(1);
        if (area)
        {
            area.destroyBottle(this);
        }
        player.getInteractableObject(gameObject);

        //}
    }
    public override void hitSomething(GameObject obj)
    {
        print("bottleHit");
        //if (anim != null)
        //    anim.Play("bulletDestroy");

        //Animacion o algo
        if (obj.GetComponent<ACharacterLife>() != null)
        {
            // hitParticle.Play();
            musicManager.Instance.PlaySoundPitch("snd_contacto_enemigo");
            musicManager.Instance.PlaySoundPitch("snd_botellarompe");
        }
        else
        {
            //impactParticle.Play();
            musicManager.Instance.PlaySoundPitch("snd_botellarompe");
        }
        moving = false;
        Destroy(gameObject, 0.5f);
    }

    public void Action(ACharacterLife shooter, float angle)
    {
        //shooter.GetComponentInChildren<IGun>().getAnim().Play("bottleThrow");
        shooter.GetComponent<PlayerShoot>().throwObject();
    }

    public void setAsEquipment(Transform equipmentParent)
    {

        transform.parent = equipmentParent;
        transform.localPosition = positionWhenEquipped;
        transform.localEulerAngles = rotationWhenEquipped;
    }
}