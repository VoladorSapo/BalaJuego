public class botleDetector : ObjectDetector<Throwable>
{
    public override void Hover(Throwable obj)
    {
        obj.setHover(true);
    }
    public override void UnHover(Throwable obj)
    {
        obj.setHover(false);
    }
}