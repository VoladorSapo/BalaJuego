using UnityEngine;
using UnityEngine.Assertions;

public class baseGun : MonoBehaviour, IGun
{

    [SerializeField] int currentBullets;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Animator anim;

    [SerializeField] bool shooting;

    [SerializeField] GameObject bullet;

    private void Start()
    {
        Assert.IsNotNull(bullet);
        Assert.IsNotNull(bullet.GetComponent< IBullet>());
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
        
    }
    public void endShootAnim()
    {
        shooting = false;
    }
}
