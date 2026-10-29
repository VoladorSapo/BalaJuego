public class baseBullet : EffectSource
{
    public void grabBullet(PlayerInteractor player)
    {
        print("grab bullet");
        player.shoot.addBullets(1);
        player.getBullet();
        Destroy(gameObject);

    }
}