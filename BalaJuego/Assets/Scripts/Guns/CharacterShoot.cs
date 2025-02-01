using UnityEngine;
using UnityEngine.Assertions;
using TMPro;

public class CharacterShoot : MonoBehaviour,IShoot
{

    [SerializeField] int startBullets;
    [SerializeField] int currentBullets;


    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject character;
    [SerializeField] public Animator anim { get; private set; }

    [SerializeField] bool shooting;

    [SerializeField] GameObject bullet;

   [SerializeField] TMP_Text bulletCount;

    gunRotate rotate;
    private void Awake()
    {
        Assert.IsNotNull(bullet);
        Assert.IsNotNull(bullet.GetComponent< IBullet>());
        rotate = GetComponent<gunRotate>();
        anim = GetComponent<Animator>();
        if (bulletCount != null)
        {
            bulletCount.text = currentBullets.ToString();
        }

    }

    public void addBullets(int bul)
    {
        currentBullets += bul;
        bulletCount.text = currentBullets.ToString();

    }

    public bool shoot()
    {
        if (currentBullets > 0 && !shooting)
        {
            shooting = true;
            currentBullets--;
            bulletCount.text = currentBullets.ToString();
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
    public void spawnBullet()
    {
     IBullet bul =   Instantiate(bullet, spawnPoint.position, Quaternion.identity).GetComponent<IBullet>();
        bul.InstantiateBullet(character.GetComponent<CharacterLife>(),rotate.transform.eulerAngles.z);
    }
    public void endShootAnim()
    {
        shooting = false;
    }
    public void restart()
    {
        shooting = false;
        currentBullets = startBullets;
        bulletCount.text = currentBullets.ToString();
        anim.Play("gunIdle");

    }
    public int getBullets() => currentBullets;

    public void setBullets(int bul)
    {
        currentBullets = bul;
    }

    public Animator getAnim() => anim;
}
