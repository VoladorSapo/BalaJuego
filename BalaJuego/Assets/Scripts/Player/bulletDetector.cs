public class bulletDetector: ObjectDetector<ABaseProyectile>
{
    public override void Hover(ABaseProyectile obj)
    {
        obj.setHover(true);
    }
    public override void UnHover(ABaseProyectile obj)
    {
        obj.setHover(false);
    }
}
