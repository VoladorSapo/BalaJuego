
/// <summary>
/// DEPRECATED
/// </summary>
public class botleDetector : ObjectDetector<IInteractable>
{
    public override void Hover(IInteractable obj)
    {
        obj.setOnRadius(true);
    }
    public override void UnHover(IInteractable obj)
    {
        obj.setOnRadius(false);
    }
}