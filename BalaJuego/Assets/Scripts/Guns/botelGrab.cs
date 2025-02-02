using UnityEngine;

public class botelGrab : MonoBehaviour
{
   [SerializeField] botella bul;
    [SerializeField] float HoverSize;

    private void Start()
    {
        bul = GetComponentInParent<botella>();
    }
    private void OnMouseOver()
    {
        print("aaaa");
        if (bul.inSelect)
        {
            bul.transform.localScale = new Vector3(HoverSize, HoverSize, HoverSize);
        }
    }
    private void OnMouseExit()
    {
        bul.transform.localScale = new Vector3(1, 1, 1);

    }
}