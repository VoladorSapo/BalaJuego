using UnityEngine;
using UnityEngine.Assertions;
using TMPro;

public class BaseGun : MonoBehaviour,IGun

{

    [SerializeField] int startBullets;
    [SerializeField] protected int currentBullets;


    [SerializeField] public Transform spawnPoint;
    [SerializeField] protected GameObject character;
    [SerializeField] public Animator anim { get; private set; }

    [SerializeField] protected bool shooting;

    [SerializeField] protected GameObject bullet;

   [SerializeField] protected TMP_Text bulletCount;

   protected gunRotate rotate;
    protected virtual void Awake()
    {
        Assert.IsNotNull(bullet);
        Assert.IsNotNull(bullet.GetComponent< IProyectile>());
        rotate = GetComponent<gunRotate>();
        anim = GetComponent<Animator>();
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

        if (currentBullets > 0 && !shooting)
        {
            shooting = true;
            if (GetComponentInParent<GunEnemyController>() != null)
            {
                anim.Play("gunLoad", -1, 0);

            }
            else
            {
                anim.Play("Gunshot", -1, 0);
            }
            return true;
        }
        return false;
    }
    public virtual void spawnBullet()
    {

        currentBullets--;
        bulletCount.text = currentBullets.ToString();
        IProyectile bul =   Instantiate(bullet, spawnPoint.position, Quaternion.identity).GetComponent<IProyectile>();
        bul.InstantiateBullet(character.GetComponent<CharacterLife>(),rotate.transform.eulerAngles.z);
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

    public void Action() => shoot();
}