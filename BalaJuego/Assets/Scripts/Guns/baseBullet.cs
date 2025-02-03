using UnityEngine;

public class baseBullet: MonoBehaviour, IBullet
{
    Vector3 direction;
    [SerializeField] public float speed;
    [SerializeField] int damage = 1;
    [SerializeField] bool grabable = true;
    [SerializeField] float lifeTime;
    [SerializeField] bool canHurtAll;

    [SerializeField] bool Infinite;

    public bool hit;
  [SerializeField]  float z;
 protected   CharacterLife.Team team;

    [SerializeField] protected ParticleSystem hitParticle;
    [SerializeField]protected  ParticleSystem impactParticle;

  public  bool inSelect;

    [SerializeField] bool onFire;

   protected float timeMagnitude;

  [SerializeField]  LayerMask obstacleLayer;

 [SerializeField] protected  Animator anim;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((obstacleLayer & (1 << collision.gameObject.layer)) != 0)
        {
            hitSomething(collision.gameObject);
        }
    }
    private void Update()
    {
        if (!Infinite)
        {
            lifeTime -= Time.deltaTime * timeMagnitude;
            if (lifeTime <= 0)
            {
                Destroy(gameObject);
            }
        }
        transform.Translate(Vector2.left * speed * Time.deltaTime*timeMagnitude);
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }
    public virtual void InstantiateBullet(CharacterLife shooter,float angle)
    {
        musicManager.Instance.PlayDisparo();
        print(shooter.transform.localScale.x);
        angle *= shooter.transform.localScale.x;
        transform.eulerAngles = new Vector3(0, shooter.transform.localScale.x < 0 ? -180 : 0, angle);
        team = shooter.team;
        if(anim){
            anim.Play("fly");
        }

    }

    public int getDamage() => damage;

 public   void changeTimeMagnitude(object sender, timeData data)
    {
        timeMagnitude = data.currentMagnitude;
    }
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        ITimeManager time = ServiceLocator.Instance.Get<ITimeManager>();
        timeMagnitude = time.getMagnitude();
        time.subscribeToTimeChange(changeTimeMagnitude);
        setHover(false);
        hit = false;

    }

    public virtual void hitSomething(GameObject obj)
    {
        //Animacion o algo
        hit = true;
        anim.Play("bulletDestroy");
       if(obj.GetComponent<CharacterLife>() != null){
            if(hitParticle)
            hitParticle.Play();
            musicManager.Instance.PlaySoundPitch("snd_contacto_enemigo");
        }
        else
        {
            if(impactParticle)
            impactParticle.Play();
            musicManager.Instance.PlaySoundPitch("snd_contacto_obstaculo");
        }
        speed = 0;
        GetComponent<Collider2D>().enabled = false;
        ServiceLocator.Instance.Get<IsoftLock>().checkAll();
        Destroy(gameObject, 0.5f);
    }
  
    public CharacterLife.Team getTeam() => team;

    public bool hurtAll() => canHurtAll;

    public virtual void tryGrab(PlayerShoot player)
    {
        player.shoot.addBullets(1);
        ServiceLocator.Instance.Get<ITimeManager>().changeTimeMagnitude(1);
        Destroy(gameObject);
        
    }
    private void OnDestroy()
    {
        ServiceLocator.Instance.Get<ITimeManager>().unSubscribeToTimeChange(changeTimeMagnitude);

    }

    public virtual void setHover(bool set)
    {
        if (tag != "Botella" && !onFire)
        {
            int setI = set ? 1 : 0;
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            GetComponentInChildren<SpriteRenderer>().GetPropertyBlock(block, 0);
            block.SetInt("_isOutlined", setI);
            print(GetComponentInChildren<SpriteRenderer>().name);
            GetComponentInChildren<SpriteRenderer>().SetPropertyBlock(block, 0);
            inSelect = set;
            if (!set)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
    //private void OnMouseOver()
    //{
    //    print("aaaa");
    //    if (inSelect)
    //    {
    //        transform.localScale = new Vector3(HoverSize, HoverSize, HoverSize);
    //    }
    //}
    //private void OnMouseExit()
    //{
    //    transform.localScale = new Vector3(1, 1, 1);

    //}

    public GameObject getObj() => gameObject;
}
