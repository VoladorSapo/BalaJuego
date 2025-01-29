using UnityEngine;
using UnityEngine.Assertions;
using TMPro;

public class CharacterShoot : MonoBehaviour,IShoot
{

    [SerializeField] int currentBullets;
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject character;
    [SerializeField] public Animator anim { get; private set; }

    [SerializeField] bool shooting;

    [SerializeField] GameObject bullet;

   [SerializeField] TMP_Text bulletCount;

    gunRotate rotate;
    private void Start()
    {
        Assert.IsNotNull(bullet);
        Assert.IsNotNull(bullet.GetComponent< IBullet>());
        rotate = GetComponent<gunRotate>();
        anim = GetComponent<Animator>();
        bulletCount.text = currentBullets.ToString();

    }

    public void addBullets(int bul)
    {
        currentBullets += bul;
        bulletCount.text = currentBullets.ToString();

    }

    public void shoot()
    {
        if (currentBullets > 0 && !shooting)
        {
            shooting = true;
            currentBullets--;
            bulletCount.text = currentBullets.ToString();
            anim.Play("Gunshot",-1,0);
        }
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

    public int getBullets() => currentBullets;

    public void setBullets(int bul)
    {
        currentBullets = bul;
    }
}
