using UnityEngine;

public class gunRotate : MonoBehaviour
{
    [SerializeField] bool followMouse;
    [SerializeField] Transform fullCharacter;

    [SerializeField] Animator anim;

    [SerializeField] Transform notTurn;

    bool shoulRotate;
    private void FixedUpdate()


    {
        if (followMouse && Time.timeScale > 0 && shoulRotate )
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

        Vector3 gunDirection = obj - transform.position;
        float angle = Mathf.Round(Mathf.Atan2(gunDirection.y, Mathf.Abs(gunDirection.x)) * Mathf.Rad2Deg);
        // print(obj +" "+ fullCharacter.position + " "+direction+" "+angle);
        if (direction.x > 0)
        {
            anim.SetBool("direction", true);

            fullCharacter.localScale = new Vector3(-1, 1, 1);
            notTurn.eulerAngles = new Vector3(notTurn.eulerAngles.x, 180, notTurn.eulerAngles.z);

        }
        else if (direction.x < 0)
        {
            anim.SetBool("direction", false);

            fullCharacter.localScale = new Vector3(1, 1, 1);
            notTurn.eulerAngles = new Vector3(notTurn.eulerAngles.x, 0, notTurn.eulerAngles.z);
        }
        Vector3 reference = direction.x > 0 ? Vector3.forward : Vector3.back;

        transform.rotation = Quaternion.AngleAxis(angle, reference);


    }
    private void Start()
    {
        ServiceLocator.Instance.Get<IGameState>().subscribeToStateChange(changeState);
        shoulRotate = false;
    }
    void changeState(object sender, stateData data)
    {
        switch (data.currentState)
        {
            case IGameState.gameState.SlowDown:
                shoulRotate = true;


                break;
            case IGameState.gameState.NormalTime:
                shoulRotate = true;


                break;
            case IGameState.gameState.Tutorial:
                shoulRotate = true;
                break;
            default:
                shoulRotate = false;
                break;
        }
    }
}