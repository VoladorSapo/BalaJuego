using UnityEngine;
using UnityEngine.EventSystems;

public class HoverOutline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color outlineColor = Color.white;

    private static readonly int OutlineSizeId = Shader.PropertyToID("_OutlineSize");
    private static readonly int OutlineColorId = Shader.PropertyToID("_OutlineColor");

    [SerializeField] SpriteRenderer sr;
    private MaterialPropertyBlock mpb;

    private void Awake()
    {
        mpb = new MaterialPropertyBlock();
        SetOutline(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("ENTER HOVER OUTLINE TEST");
        SetOutline(true);
    }
    public void OnPointerExit(PointerEventData eventData) => SetOutline(false);

    private void OnDisable() => SetOutline(false);

    private void SetOutline(bool on)
    {
        sr.GetPropertyBlock(mpb);               // conserva lo que ya puso el SpriteRenderer (_MainTex)
        mpb.SetColor(OutlineColorId, outlineColor);
        mpb.SetFloat(OutlineSizeId, on ? 1f : 0f);
        sr.SetPropertyBlock(mpb);
    }
}