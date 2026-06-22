public class playerGun: BaseGun
{
    PlayerShoot playerShoot;

    
    public override void endShootAnim()
    {
        base.endShootAnim();
        playerShoot.endShootAnim();
    }
    protected override void Awake()
    {
        base.Awake();
        playerShoot = gameObject.GetComponentInParent<PlayerShoot>();
    }
}