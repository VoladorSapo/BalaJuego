using UnityEngine;

public class gunRotate:MonoBehaviour
{
    [SerializeField] bool followMouse;
 [SerializeField]   Transform fullCharacter;
    void Update()
    {
        if (followMouse && Time.timeScale > 0)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos.z = 10;
            setRotation(Camera.main.ScreenToWorldPoint(mousePos));
        }
    }
    public void setRotation(Vector3 obj)
    {

        Vector3 direction = obj - fullCharacter.position;
        float angle = Mathf.Atan2(direction.y, Mathf.Abs(direction.x)) * Mathf.Rad2Deg;
        print(obj +" "+ fullCharacter.position + " "+direction+" "+angle);
        if (direction.x > 0)
        {
            fullCharacter.localScale = new Vector3(-1, 1, 1);
        }
        else if (direction.x < 0)
        {
            fullCharacter.localScale = new Vector3(1, 1, 1);
        }
        Vector3 reference = direction.x > 0 ? Vector3.forward : Vector3.back;

        transform.rotation = Quaternion.AngleAxis(angle, reference);

       
    }
}
