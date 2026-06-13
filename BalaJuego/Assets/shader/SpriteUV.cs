using UnityEngine;

[ExecuteInEditMode] // Para que también funcione en el editor de Unity
public class SpriteUVBridge : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private MaterialPropertyBlock propertyBlock;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    void LateUpdate()
    {
        if (spriteRenderer == null || spriteRenderer.sprite == null) return;

        // Obtenemos los límites UV exactos (Min y Max) del frame actual de la animación
        Vector4 uvRect = UnityEngine.Sprites.DataUtility.GetOuterUV(spriteRenderer.sprite);
        // uvRect.x = Min X, uvRect.y = Min Y, uvRect.z = Max X, uvRect.w = Max Y

        spriteRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetVector("_SpriteUVRect", uvRect);
        spriteRenderer.SetPropertyBlock(propertyBlock);
    }
}