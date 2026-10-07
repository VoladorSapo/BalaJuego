using UnityEngine;

public class SpriteHover : ABaseHover
{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Sprite hoverSprite;
    Sprite ogSprite;


    private void Start()
    {
        ogSprite = sprite.sprite;
    }
    public override void setHover(bool set)
    {
        sprite.sprite = set ? hoverSprite : ogSprite;
    }
}
