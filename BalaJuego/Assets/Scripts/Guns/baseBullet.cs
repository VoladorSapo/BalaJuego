public class baseBullet : ABaseProyectile
{
    public override void tryGrab(PlayerShoot player)
    {
        player.shoot.addBullets(1);
        player.getBullet();
        ServiceLocator.Instance.Get<ITimeManager>().changeTimeMagnitude(1);
        Destroy(gameObject);

    }
}
