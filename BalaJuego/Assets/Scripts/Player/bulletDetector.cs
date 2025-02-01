public class bulletDetector: ObjectDetector<baseBullet>
{
    public override void Hover(baseBullet obj)
    {
        obj.setHover(true);
    }
    public override void UnHover(baseBullet obj)
    {
        obj.setHover(false);
    }
}
