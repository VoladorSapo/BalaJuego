using UnityEngine;

public class SizeHover : ABaseHover
{
    [SerializeField] SpriteRenderer sprite;
    private MaterialPropertyBlock mpb;

    private static readonly int OutlineSizeId = Shader.PropertyToID("_OutlineSize");
    private static readonly int OutlineColorId = Shader.PropertyToID("_OutlineColor");

    [SerializeField] private Color outlineColor = Color.white;

    [SerializeField] float HoverSize;
    Vector3 ogSize;
    Vector3 hoveredSize;

    private void Awake()
    {
        mpb = new MaterialPropertyBlock();
        SetOutline(false);
    }
    private void Start()
    {
        ogSize = sprite.transform.localScale;
        hoveredSize = new Vector3(ogSize.x * HoverSize, ogSize.y * HoverSize, ogSize.z * HoverSize);
    }
    public override void setHover(bool set)
    {
        if (sprite.transform.parent != null)
        {
            sprite.transform.parent.localScale = set ? hoveredSize : ogSize;
        }
        else
        {
            sprite.transform.localScale = set ? hoveredSize : ogSize;
        }
        SetOutline(set);
    }

    private void SetOutline(bool on)
    {
        sprite.GetPropertyBlock(mpb);               // conserva lo que ya puso el SpriteRenderer (_MainTex)
        mpb.SetColor(OutlineColorId, outlineColor);
        mpb.SetFloat(OutlineSizeId, on ? 1f : 0f);
        sprite.SetPropertyBlock(mpb);
    }
}
