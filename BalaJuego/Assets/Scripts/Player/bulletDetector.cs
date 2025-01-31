public class bulletDetector: ObjectDetector<IBullet>
{
    public override void Hover(IBullet obj)
    {
        obj.setHover(true);
    }
    public override void UnHover(IBullet obj)
    {
        obj.setHover(false);
    }
}
