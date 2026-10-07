using UnityEngine;

public class ColorHover : ABaseHover
{
    [SerializeField] SpriteRenderer sprite;

    [SerializeField] Color hoverColor = Color.yellow;
   Color ogColor;

    private void Start()
    {
        ogColor = sprite.color;
    }
    public override void setHover(bool set)
    {
        sprite.color = set ? hoverColor : ogColor;
    }
}