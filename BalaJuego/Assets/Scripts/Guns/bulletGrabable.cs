using System;
using UnityEngine;
public class bulletGrabable : MonoBehaviour
{
    ABaseProyectile bul;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] float HoverSize;
    Vector3 ogSize;
    Vector3 hoveredSize;
    private void Start()
    {
        bul = GetComponentInParent<ABaseProyectile>();
        ogSize = sprite.transform.localScale;
        hoveredSize = new Vector3(ogSize.x * HoverSize, ogSize.y * HoverSize, ogSize.z * HoverSize);
    }
    private void OnMouseOver()
    {
        if (bul.inSelect)
        {
            print("mouseEnter");
            sprite.transform.localScale = hoveredSize;
        }
    }
    private void OnMouseExit()
    {
        print("mouseExit");
        sprite.transform.localScale = ogSize;
    }
}
