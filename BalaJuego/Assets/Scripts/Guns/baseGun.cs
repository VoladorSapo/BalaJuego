using UnityEngine;
using UnityEngine.Assertions;

public class baseGun : MonoBehaviour, IGun
{

    [SerializeField] int currentBullets;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Animator anim;

    [SerializeField] bool shooting;

    [SerializeField] GameObject bullet;

    gunRotate rotate;
    private void Start()
    {
        Assert.IsNotNull(bullet);
        Assert.IsNotNull(bullet.GetComponent< IBullet>());
        rotate = GetComponent<gunRotate>();
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
        }
    }
    public void spawnBullet()
    {
     IBullet bul =   Instantiate(bullet, spawnPoint.position, Quaternion.identity).GetComponent<IBullet>();
        bul.InstantiateBullet(rotate.transform.eulerAngles);
    }
    public void endShootAnim()
    {
        shooting = false;
    }
}
