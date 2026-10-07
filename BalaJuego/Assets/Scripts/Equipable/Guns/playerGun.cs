public class playerGun: BaseGun
{
    PlayerInteractor playerShoot;

    
    public override void endShootAnim()
    {
        base.endShootAnim();
        playerShoot.endShootAnimation();
    }
    protected override void Awake()
    {
        base.Awake();
        playerShoot = gameObject.GetComponentInParent<PlayerInteractor>();
    }
}