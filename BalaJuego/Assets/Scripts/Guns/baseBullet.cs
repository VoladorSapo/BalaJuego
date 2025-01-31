using UnityEngine;

public class baseBullet: MonoBehaviour, IBullet
{
    Vector3 direction;
    [SerializeField] float speed;
    [SerializeField] int damage = 1;
    [SerializeField] bool grabable = true;
    [SerializeField] float lifeTime;
    [SerializeField] bool canHurtAll;

    [SerializeField] bool Infinite;

    CharacterLife.Team team;

    [SerializeField] ParticleSystem hitParticle;
    [SerializeField] ParticleSystem impactParticle;

    bool inSelect;

    [SerializeField] float HoverSize;


    float timeMagnitude;

  [SerializeField]  LayerMask obstacleLayer;



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
    }
    public void InstantiateBullet(CharacterLife shooter,float angle)
    {
        musicManager.Instance.PlayDisparo();
        print(shooter.transform.localScale.x);
        angle *= shooter.transform.localScale.x;
        transform.eulerAngles = new Vector3(0, shooter.transform.localScale.x < 0 ? -180 : 0, angle);
        team = shooter.team;
    }

    public int getDamage() => damage;

    void changeTimeMagnitude(object sender, timeData data)
    {
        timeMagnitude = data.currentMagnitude;
    }
    private void Start()
    {
        ITimeManager time = ServiceLocator.Instance.Get<ITimeManager>();
        timeMagnitude = time.getMagnitude();
        time.subscribeToTimeChange(changeTimeMagnitude);
        setHover(false);

    }

    public void hitSomething(GameObject obj)
    {
        //Animacion o algo
       if(obj.GetComponent<CharacterLife>() != null){
            hitParticle.Play();
            musicManager.Instance.PlaySoundPitch("snd_contacto_enemigo");
        }
        else
        {
            impactParticle.Play();
            musicManager.Instance.PlaySoundPitch("snd_contacto_obstaculo");
        }
        speed = 0;
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

    public void setHover(bool set)
    {
        int setI = set ? 1 : 0;
        MaterialPropertyBlock block = new MaterialPropertyBlock();
      GetComponentInChildren<SpriteRenderer>().GetPropertyBlock(block,0);
        block.SetInt("_isOutlined", setI);
        print(GetComponentInChildren<SpriteRenderer>().name);
        GetComponentInChildren<SpriteRenderer>().SetPropertyBlock(block,0);
        inSelect = set;
        if (!set)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    private void OnMouseOver()
    {
        print("aaaa");
        if (inSelect)
        {
            transform.localScale = new Vector3(HoverSize, HoverSize, HoverSize);
        }
    }
    private void OnMouseExit()
    {
        transform.localScale = new Vector3(1, 1, 1);

    }
}