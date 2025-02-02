using UnityEngine;

public class BossShoot : CharacterShoot
{
    [SerializeField] GameObject fireBullet;
    [SerializeField] int specialBulletRate;
    int currentRate;


    public override void restart()
    {
        base.restart();
        currentRate = 0;
    }
    public override bool shoot()
    {
        if (currentBullets > 0 && !shooting)
        {
            shooting = true;
            if (GetComponentInParent<GunEnemyController>() != null || GetComponentInParent<BossEnemyController>() != null)
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
    public override void spawnBullet()
    {
        currentBullets--;
        bulletCount.text = currentBullets.ToString();

        IBullet bul = null;
        if (currentRate < specialBulletRate)
        {
            bul = Instantiate(bullet, spawnPoint.position, Quaternion.identity).GetComponent<IBullet>();
            currentRate++;
        }
        else
        {
            bul = Instantiate(fireBullet, spawnPoint.position, Quaternion.identity).GetComponent<IBullet>();
            currentRate = 0;
        }
        bul.InstantiateBullet(character.GetComponent<CharacterLife>(), rotate.transform.eulerAngles.z);
    }
    public override void addBullets(int bul)
    {
        base.addBullets(bul);
        currentRate = 0;


    }
}