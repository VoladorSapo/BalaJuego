using UnityEngine;
using UnityEngine.Assertions;

public class CharacterShoot : MonoBehaviour,IShoot
{

    [SerializeField] int currentBullets;
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject character;
    [SerializeField] Animator anim;

    [SerializeField] bool shooting;

    [SerializeField] GameObject bullet;

    gunRotate rotate;
    private void Start()
    {
        Assert.IsNotNull(bullet);
        Assert.IsNotNull(bullet.GetComponent< IBullet>());
        rotate = GetComponent<gunRotate>();
        anim = GetComponent<Animator>();
    }

    public void addBullets(int bul)
    {
        currentBullets += bul;
    }

    public void shoot()
    {
        if (currentBullets > 0 && !shooting)
        {
            shooting = true;
            currentBullets--;
            anim.Play("playerGunshot",-1,0);
        }
    }
    public void spawnBullet()
    {
     IBullet bul =   Instantiate(bullet, spawnPoint.position, Quaternion.identity).GetComponent<IBullet>();
        bul.InstantiateBullet(character,rotate.transform.eulerAngles.z);
    }
    public void endShootAnim()
    {
        shooting = false;
    }
}
