using UnityEngine;

public enum HeadLookingState { up, mid, down };
public abstract class AHeadSpriteController : MonoBehaviour
{
    [System.Serializable]
    public struct Face
    {
        public string faceName;
        public Sprite spriteUp, spriteMid, spriteDown;
        public Sprite GetSprite(HeadLookingState lookingAt)
        {
            switch (lookingAt)
            {
                case HeadLookingState.up:
                    return spriteUp;
                case HeadLookingState.mid:
                    return spriteMid;
                case HeadLookingState.down:
                    return spriteDown;
                default:
                    return spriteMid;
            }
        }
    }

    [Header("Set by animation")]
    public int currentFace;

    [Header("Faces database")]
    SpriteRenderer spriteRenderer;
    [SerializeField] Face[] faces;

    protected HeadLookingState lookingAt = HeadLookingState.mid;

    protected private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected virtual void LateUpdate()
    {

        spriteRenderer.sprite = faces[currentFace].GetSprite(lookingAt);

    }

}
