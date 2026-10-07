using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectSource : MonoBehaviour, IEffectSource
{
    [Header("Data")]
    [SerializeField] bool canHurtAll;

        [SerializeField] protected bool destroyOnHit = true;




    [SerializeField] bool canHitDefault;

   [SerializeField] protected ACharacterLife owner;
    [SerializeField] protected HittableType hitType = HittableType.onlyOtherTeam;


    [Header("Particles")]
    [SerializeField] Transform collisionParticleParent;

    [SerializeField] protected ParticleSystem hitParticle;
    [SerializeField] protected ParticleSystem impactParticle;

  //  public bool inSelect;


    [SerializeField] LayerMask obstacleLayer;

    [SerializeField] protected Animator anim;
    [SerializeField] private EffectEditor[] effects;

    IProyectile proyectile;

    [Header("Debug")]
    [SerializeField] bool canHit;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canHit)
        {
            return;  
        }

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



    private void Awake()
    {
       // setHover(false);
        canHit = canHitDefault;
        proyectile = GetComponent<IProyectile>();
        if (proyectile != null)
        {
            proyectile.setActivateEvent(setInteractor);
        }
    }

    protected virtual void Start()
    {

        
       

    }
    public virtual void hitSomething(GameObject obj)
    {
        print("hit " +gameObject.name);
        //Animacion o algo
        
        if (obj.GetComponent<ACharacterLife>() != null)
        {
            if (hitParticle)
            {
                float bulletAngle = transform.eulerAngles.z;
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
        }
        else
        {
            if (impactParticle)
                impactParticle.Play();
            musicManager.Instance.PlaySoundPitch("snd_contacto_obstaculo");
        }
        ServiceLocator.Instance.Get<IsoftLock>().checkAll();
        if (proyectile != null)
        {
            proyectile.hitSomething(obj);
        }
        if (destroyOnHit)
        {
            GetComponent<Collider2D>().enabled = false;
            // print("destroy");
            canHit = false;
            Destroy(gameObject, 0.5f);
            if (anim)
            {
                anim.Play("bulletDestroy");
            }
        }
    }
  

    public ACharacterLife.Team getTeam() => owner.GetComponent<ACharacterLife>().team;

    public bool hurtAll() => canHurtAll;

    private void OnDestroy()
    {
     //   print("DestroyBullet"+gameObject.name);
        ServiceLocator.Instance.Get<IsoftLock>().checkAll();
    }

    //public virtual void setHover(bool set)
    //{
    //    inSelect = set;
    //    int setI = set ? 1 : 0;
    //    //MaterialPropertyBlock block = new MaterialPropertyBlock();
    //    //GetComponentInChildren<SpriteRenderer>().GetPropertyBlock(block, 0);
    //    //block.SetInt("_isOutlined", setI);
    //    //print(GetComponentInChildren<SpriteRenderer>().name
    //    //GetComponentInChildren<SpriteRenderer>().SetPropertyBlock(block, 0);
    //    if (!set)
    //    {
    //        transform.localScale = new Vector3(1, 1, 1);
    //    }
    //}
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



    public ACombatEffect[] getEffects()
    {
        return effects.Select(effect => effect.createEffect()).ToArray();
    }


    
    public void ActivateSource(bool activate)
    {
        GetComponent<Collider2D>().enabled = activate;
        canHit = activate;
    }

    public void setInteractor(CharacterInteractor shooter)
    {
        owner = shooter.GetComponent<ACharacterLife>();
        ActivateSource(true);
    }
}
public enum HittableType
{
    allCharactersNoMe,
    allCharacters,
    onlyOtherTeam,
    onlyPlayer
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
            case HittableType.onlyPlayer:
                return objective.GetComponent<PlayerLife>() != null;
        }
        return false;
    }
}

[System.Serializable]
public class EffectEditor
{
    public EffectTypes type;
    public int stat;
    public bool infinite;
    public float duration;
    public ACombatEffect createEffect()
    {
        switch (type)
        {
            case EffectTypes.Damage:
                return new DamageEffect(stat);
            case EffectTypes.AddShield:
                return new HatEffect(stat);
            case EffectTypes.Stun:
                return new StunEffect(infinite,duration);
        }
        return new DamageEffect(stat);
    }
}
public enum EffectTypes
{
    Damage,
    AddShield,
    Stun
}
