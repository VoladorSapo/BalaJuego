using System.Collections.Generic;
using UnityEngine;

public class baseBullet : MonoBehaviour, IInteractable, IProyectile
{
    [Header("Data")]
    [SerializeField] bool grabable = true;
    [field: SerializeField] public float speed { get; protected set; }
    [SerializeField] int damage = 1;
    [SerializeField] float lifeTime;
    [SerializeField] bool canHurtAll;

    [SerializeField] bool Infinite;
    [SerializeField] bool onFire;

    [SerializeField] Transform collisionParticleParent;
    [Header("Debug")]
    Vector3 direction;

    [field: SerializeField] public bool moving { get; protected set; }

    public bool hit;
    [SerializeField] float z;
    protected ACharacterLife owner;
    [SerializeField] protected HittableType hitType;


    [SerializeField] protected ParticleSystem hitParticle;
    [SerializeField] protected ParticleSystem impactParticle;

    public bool inSelect;


    protected float timeMagnitude;

    [SerializeField] LayerMask obstacleLayer;

    [SerializeField] protected Animator anim;
    [SerializeField] private EffectEditor effect;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        IHittable hittable = collision.gameObject.GetComponent<IHittable>();
        if (hittable != null)
        {
            if (hittable.getHit(this))
            {
                hitSomething(collision.gameObject);

            }
        }
        if ((obstacleLayer & (1 << collision.gameObject.layer)) != 0)
        {
            if (collision.TryGetComponent<MaterialInfo>(out MaterialInfo materialInfo))
            {
                if (materialInfo.material.bulletImpactParticles != null)
                    Instantiate(materialInfo.material.bulletImpactParticles, collisionParticleParent);
            }
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
                ServiceLocator.Instance.Get<IsoftLock>().checkAll();
                Destroy(gameObject);
            }
        }
        if (moving)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime * timeMagnitude);
            transform.position = new Vector3(transform.position.x, transform.position.y, z);
        }
    }
    public virtual void InstantiateBullet(ACharacterLife shooter, float angle)
    {
        musicManager.Instance.PlayDisparo();
        print(shooter.transform.localScale.x);
        angle *= shooter.transform.localScale.x;
        transform.eulerAngles = new Vector3(0, shooter.transform.localScale.x < 0 ? -180 : 0, angle);
        owner = shooter;
        moving = true;
        if (anim)
        {
            anim.Play("fly");
        }
    }
    public virtual void InstantiateBullet(ACharacterLife shooter, float angle, Vector3 pos)
    {
        InstantiateBullet(shooter, angle);
        transform.position = new Vector3(pos.x, pos.y, z);
    }

    public int getDamage() => damage;

    public void changeTimeMagnitude(object sender, timeData data)
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
        print("hit");
        //Animacion o algo
        hit = true;
        if (anim)
        {
            anim.Play("bulletDestroy");
        }
        if (obj.GetComponent<ACharacterLife>() != null)
        {
            if (hitParticle)
            {
                float bulletAngle = transform.eulerAngles.z;
                //float rad = bulletAngle * Mathf.Deg2Rad;
                //Vector2 bulletDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
                var particles = obj.GetComponentInChildren<ParticleSystem>(true);
                if (particles != null)
                {
                    particles.gameObject.SetActive(true);
                    particles.Play();
                    if (transform.eulerAngles.z < 10 && transform.eulerAngles.z > -10)
                    {
                        particles.transform.eulerAngles = new Vector3(-25, -particles.transform.eulerAngles.y, -particles.transform.eulerAngles.z);
                    }
                    else

                        particles.transform.eulerAngles = new Vector3(transform.eulerAngles.z, -particles.transform.eulerAngles.y, -particles.transform.eulerAngles.z);
                }

                musicManager.Instance.PlaySoundPitch("snd_contacto_enemigo");
            }
            else
            {
                if (impactParticle)
                    impactParticle.Play();
                musicManager.Instance.PlaySoundPitch("snd_contacto_obstaculo");
            }
            moving = false;
            GetComponent<Collider2D>().enabled = false;
        }
        ServiceLocator.Instance.Get<IsoftLock>().checkAll();
        Destroy(gameObject, 0.5f);


    }
    public ACharacterLife.Team getTeam() => owner.team;

    public bool hurtAll() => canHurtAll;

    public virtual void tryGrab(PlayerShoot player)
    {
        player.shoot.addBullets(1);
        player.getBullet();
        ServiceLocator.Instance.Get<ITimeManager>().changeTimeMagnitude(1);
        Destroy(gameObject);

    }
    private void OnDestroy()
    {
        ServiceLocator.Instance.Get<IsoftLock>().checkAll();
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

    public ACharacterLife getOwner() => owner;

    public HittableType getHittableType() => hitType;


    public ACombatEffect getEffect() => effect.createEffect();
}

public enum HittableType
{
    allCharactersNoMe,
    allCharacters,
    onlyOtherTeam,
}
public class HittableCheck
{
    public static bool checkHit(ACharacterLife objective, ACharacterLife origin, HittableType hitType)
    {
        switch (hitType)
        {
            case HittableType.allCharactersNoMe:
                return objective != origin;
            case HittableType.allCharacters:
                return true;
            case HittableType.onlyOtherTeam:
                return objective.team != origin.team;
        }
        return false;
    }
}

[System.Serializable]
public class EffectEditor
{
    public int type;
    public ACombatEffect createEffect()
    {
        if (type == 0)
        {
            return new DamageEffect(1);
        }
        else
        {
            return new StunEffect(2);
        }
    }
}