public class playerGunRotate : gunRotate
{
    protected override void Start()
    {
        base.Start();
        GetComponentInParent<PlayerInteractor>().subscribeToPlayerGunChange(playerGunChange);
    }

    private void playerGunChange(characterGunChangeData data)
    {
        shouldRotate = data.hasSomething;
    }
}