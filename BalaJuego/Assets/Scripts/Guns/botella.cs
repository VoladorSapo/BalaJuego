using UnityEngine;

public class botella : baseBullet,IEquipable
{
    public bool isThrown;
  [SerializeField]  LevelAreaController area;
    Vector3 initialPos;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        initialPos = transform.position;
        ITimeManager time = ServiceLocator.Instance.Get<ITimeManager>();
        timeMagnitude = time.getMagnitude();
        hit = false;
    }

    public void resTart(LevelAreaController _area)
    {
        
        print("botella area" + _area);
        area = _area;
        transform.position = initialPos;

    }
    public override void setHover(bool set)
    {
        print("setHover" + set);
            int setI = set ? 1 : 0;
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            GetComponentInChildren<SpriteRenderer>().GetPropertyBlock(block, 0);
            block.SetInt("_isOutlined", setI);
            print(GetComponentInChildren<SpriteRenderer>().name);
            GetComponentInChildren<SpriteRenderer>().SetPropertyBlock(block, 0);
            inSelect = set;
            musicManager.Instance.PlaySoundPitch("bip", 0.2f);
        if (!set)
        {
            transform.localScale = new Vector3(1, 1, 1);

        }
    }
    public override void InstantiateBullet(ACharacterLife shooter, float angle)
    {
        transform.parent= null;
        musicManager.Instance.PlaySoundPitch("snd_lanzabotella");
        print(shooter.transform.localScale.x);
        angle *= shooter.transform.localScale.x;
        GetComponent<CapsuleCollider2D>().enabled = true;
        transform.eulerAngles = new Vector3(0, shooter.transform.localScale.x < 0 ? -180 : 0, angle);
        this.owner = shooter;
        anim = GetComponentInChildren<Animator>(true);
        anim.Play("bottleFly");
        moving = true; 
    }

    public override void tryGrab(PlayerShoot player)
    {
        if (area)
        {
            musicManager.Instance.PlaySoundPitch("snd_pick", 0.2f);
            ServiceLocator.Instance.Get<ITimeManager>().changeTimeMagnitude(1);
            area.destroyBottle(this);
            //gameObject.SetActive(false);
            player.getInteractableObject(gameObject);

        }
    }
    public override void hitSomething(GameObject obj)
    {
        print("bottleHit");
        anim.Play("bulletDestroy");

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
            //Destroy(gameObject, 0.5f);
        
    }

    public void Action(ACharacterLife shooter, float angle)
    {
        //shooter.GetComponentInChildren<IGun>().getAnim().Play("bottleThrow");
        shooter.GetComponent<PlayerShoot>().throwObject();
    }
}


