using UnityEngine;

public class SizeHover : ABaseHover
{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] float HoverSize;
    Vector3 ogSize;
    Vector3 hoveredSize;


    private void Start()
    {
        ogSize = sprite.transform.localScale;
        hoveredSize = new Vector3(ogSize.x * HoverSize, ogSize.y * HoverSize, ogSize.z * HoverSize);
    }
    public override void setHover(bool set)
    {
      sprite.transform.localScale = set ? hoveredSize : ogSize;
    }
}
