public class interactableDetector: ObjectDetector<IInteractable>
{
    public override void Hover(IInteractable obj)
    {
        obj.setHover(true);
    }
    public override void UnHover(IInteractable obj)
    {
        obj.setHover(false);
    }
}
