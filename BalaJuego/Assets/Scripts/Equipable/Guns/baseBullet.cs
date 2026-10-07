public class baseBullet : EffectSource
{
    public void interact(PlayerInteractor player)
    {
        print("grab bullet");
        player.shoot.addBullets(1);
        player.getBullet();
        Destroy(gameObject);

    }
}