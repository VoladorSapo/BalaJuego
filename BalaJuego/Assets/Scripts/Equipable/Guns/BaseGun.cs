using UnityEngine;
using UnityEngine.Assertions;
using TMPro;

public class BaseGun : ABaseEquipable, IGun

{

    [SerializeField] protected int startBullets;
    [SerializeField] protected int currentBullets;


    [SerializeField] public Transform spawnPoint;
    [SerializeField] protected GameObject character;
    [field:SerializeField] public Animator anim { get; private set; }

    [SerializeField] protected bool shooting;

    [SerializeField] protected GameObject bullet;

   [SerializeField] protected TMP_Text bulletCount;

   protected gunRotate rotate;

    GunEnemyBehaviour gunEnemyController;

    [SerializeField] bool DestroyWhenOutOfBullets = false;
    protected virtual void Awake()
    {
        Assert.IsNotNull(bullet);
        Assert.IsNotNull(bullet.GetComponent< IEffectSource>());
        rotate = GetComponent<gunRotate>();
        anim = GetComponentInChildren<Animator>();
        gunEnemyController = GetComponentInParent<GunEnemyBehaviour>();
        if (bulletCount != null)
        {
            bulletCount.text = currentBullets.ToString();
        }

    }

    public virtual void addBullets(int bul)
    {
        shooting = false;
        currentBullets += bul;
        bulletCount.text = currentBullets.ToString();

    }

    public virtual bool shoot()
    {

        if (!shooting)
        {
            if (currentBullets > 0)
            {
                shooting = true;
                if (gunEnemyController != null)
                {
                    gunEnemyController.playAnimation("Load");
                }
                else
                {
                    anim.Play("Gunshot", -1, 0);
                }
                return true;
            }
            runOutOfBullets();
            return false;
        }
        return false;
    }
    public virtual void spawnBullet()
    {
        print($"currentBullets {character.name}: {currentBullets}");
        currentBullets--;
        if( currentBullets <= 0)
        {
            runOutOfBullets();
        }
        bulletCount.text = currentBullets.ToString();
        IProyectile bul =   Instantiate(bullet, spawnPoint.position, Quaternion.identity).GetComponent<IProyectile>();
        print(bul == null);
        print(character == null);
        print(rotate == null);
        print(spawnPoint == null);
        bul.ActivateProyectileMovement(character.GetComponent<CharacterInteractor>(),transform.eulerAngles.z, spawnPoint.position);
    }
    
    public virtual void endShootAnim()
    {
        shooting = false;
    }
    public virtual void restart()
    {
        shooting = false;
        currentBullets = startBullets;
        bulletCount.text = currentBullets.ToString();
        anim.Play("gunIdle");

    }
    public virtual int getBullets() => currentBullets;

    public virtual void setBullets(int bul)
    {
        currentBullets = bul;
        bulletCount.text = currentBullets.ToString();
    }

    public Animator getAnim() => anim;

    public virtual void setShooting(bool _shoot)
    {
        shooting = _shoot;
    }


  


    //public void setOwner(PlayerInteractor shoot, bool resetToStartingBullets = true)
    //{
    //    bulletCount = shoot.GetComponentInChildren<TMP_Text>();
    //    if (resetToStartingBullets)
    //    {
    //        setBullets(startBullets);
    //    }
    //}
    public override void Grab(CharacterInteractor character)
    {
        this.character = character.gameObject;
        base.Grab(character);
        bulletCount = character.GetComponentInChildren<TMP_Text>();
        bulletCount.text = currentBullets.ToString();

        // setBullets(startBullets);
    }

    public override void Action(CharacterInteractor interactor, float angle)
    {
        shoot();
    }

    public virtual void runOutOfBullets()
    {
        if (DestroyWhenOutOfBullets)
        {
            DestroyEquipment();
        }
    }
}