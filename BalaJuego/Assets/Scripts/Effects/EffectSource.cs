using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

public class EffectSource : MonoBehaviour, IEffectSource
{
    [Header("Data")]
    [SerializeField] bool canHurtAll;

    [SerializeField] protected bool destroyOnHit = true;

    CinemachineImpulseSource impulseSource;


    [SerializeField] bool canHitDefault;

    [SerializeField] protected ACharacterLife owner;
    [SerializeField] protected HittableType hitType = HittableType.onlyOtherTeam;


    [Header("Particles")]
    [SerializeField] Transform collisionParticleParent;
    //  public bool inSelect;


    [SerializeField] LayerMask obstacleLayer;

    [SerializeField] protected Animator anim;
    [SerializeField] private EffectEditor[] effects;

    IProyectile proyectile;

    [Header("Debug")]
    [SerializeField] bool canHit;

    DestroyRespawnController destroyController;

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
        proyectile = GetComponent<IProyectile>();
        if (proyectile != null)
        {
            proyectile.setActivateEvent(setInteractor);
        }
        destroyController = GetComponent<DestroyRespawnController>();
        DestroyRespawnController respawnController = GetComponent<DestroyRespawnController>();
        if (respawnController != null)
            respawnController.subscribeToRespawnAction(restart);
        restart();
    }
    public virtual void restart()
    {
        canHit = canHitDefault;
    }
    protected virtual void Start()
    {

        impulseSource = GetComponent<CinemachineImpulseSource>();


    }
    public virtual void hitSomething(GameObject obj)
    {
        print("hit " + gameObject.name + " " + obj.name);
        //Animacion o algo

        if (obj.GetComponent<ACharacterLife>() != null)
        {
            Vector3 dir = -transform.right;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            var particles = obj.GetComponentsInChildren<ParticleSystem>(true)[0];
            if (particles != null)
            {
                Debug.Log($"PARTICULAS {particles.name} ROTADAS");
                var shape = particles.shape;
                particles.gameObject.SetActive(true);
                if(angle < 10 && angle > -10)
                {
                    shape.rotation = new Vector3(0, 65, 0f);
                }
                shape.rotation = new Vector3(0, 90 - angle, 0f);
                particles.Play();

            }
            ServiceLocator.Instance.Get<IHitStop>().HitStop(.05f);
            Shake(.5f, .6f);
            musicManager.Instance.PlaySoundPitch("snd_contacto_enemigo");
        }
        else
        {
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
            if (destroyController != null)
            {
                destroyController.Destroy(0.5f);
            }
            else
            {
                Destroy(gameObject, 0.5f);
            }
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

    public void Shake(float force = 1f, float delay = 0)
    {
        if (delay > 0)
        {
            StartCoroutine(ShakeDelay(force, delay));
        }
        Debug.Log("SHAKE " + force);
        impulseSource.GenerateImpulse(force);
    }

    IEnumerator ShakeDelay(float force, float delay)
    {
        yield return new WaitForSeconds(delay);
        Shake(force, 0);
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
                return new StunEffect(infinite, duration);
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
