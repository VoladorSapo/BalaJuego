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
        Vector3 direction = obj - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Vector3 reference = direction.x > 0 ? Vector3.back : Vector3.forward;

        transform.rotation = Quaternion.AngleAxis(angle, reference);

        if (direction.x > 0)
        {
            fullCharacter.localScale = new Vector3(-1, 1, 1);
        }
        else if (direction.x < 0)
        {
            fullCharacter.localScale = new Vector3(1, 1, 1);

        }
    }
}
