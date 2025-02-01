public class botleDetector : ObjectDetector<botella>
{
    public override void Hover(botella obj)
    {
        obj.setHover(true);
    }
    public override void UnHover(botella obj)
    {
        obj.setHover(false);
    }
}