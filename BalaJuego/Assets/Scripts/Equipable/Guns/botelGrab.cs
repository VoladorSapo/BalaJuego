using UnityEngine;

public class botelGrab : MonoBehaviour
{
   [SerializeField] IInteractable bul;
    [SerializeField] float HoverSize;

    private void Start()
    {
        bul = GetComponentInParent<IInteractable>();
    }
    private void OnMouseOver()
    {
        print("aaaa");
        if (bul.inSelect())
        {
            bul.getObj().transform.localScale = new Vector3(HoverSize, HoverSize, HoverSize);
        }
    }
    private void OnMouseExit()
    {
        bul.getObj().transform.localScale = new Vector3(1, 1, 1);

    }
}