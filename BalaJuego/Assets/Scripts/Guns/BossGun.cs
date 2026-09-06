using UnityEngine;

public class BossGun : BaseGun
{
    [SerializeField] GameObject specialBullet;
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
            if (GetComponentInParent<GunEnemyBehaviour>() != null || GetComponentInParent<BossEnemyController>() != null)
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

        IProyectile bul = null;
        if (currentRate < specialBulletRate)
        {
            bul = Instantiate(bullet, spawnPoint.position, Quaternion.identity).GetComponent<IProyectile>();
            currentRate++;
        }
        else
        {
            bul = Instantiate(specialBullet, spawnPoint.position, Quaternion.identity).GetComponent<IProyectile>();
            currentRate = 0;
        }
        bul.ActivateProyectileMovement(character.GetComponent<ACharacterLife>(), rotate.transform.eulerAngles.z);
    }
    public override void addBullets(int bul)
    {
        base.addBullets(bul);
        currentRate = 0;


    }
}