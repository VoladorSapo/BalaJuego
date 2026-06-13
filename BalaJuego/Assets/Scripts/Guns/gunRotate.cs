using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class gunRotate : MonoBehaviour
{
    [SerializeField] bool followMouse;
    [SerializeField] Transform fullCharacter;

    [Header("Animator")]
    //public Animator bodyAnim;
    public Animator headAnim;
    public Animator armAnim;
    public Animator parentAnim;
    string currentStm = "MID";

    [SerializeField] Transform notTurn;

    bool shoulRotate;
    private void FixedUpdate()
    {
        
        if (followMouse && Time.timeScale > 0 && shoulRotate )
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            headAnim.SetFloat("cursorY", mousePos.y);
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
            UpdateAnimatorBool("direction", true);

            fullCharacter.localScale = new Vector3(-1, 1, 1);
            notTurn.eulerAngles = new Vector3(notTurn.eulerAngles.x, 180, notTurn.eulerAngles.z);

        }
        else if (direction.x < 0)
        {
            UpdateAnimatorBool("direction", false);

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
    public void UpdateAnimatorFloat(string property, float value)
    {
        //if (bodyAnim != null) { bodyAnim.SetFloat(property, value); }
        if (headAnim != null) { headAnim.SetFloat(property, value); }
        if (armAnim != null) { armAnim.SetFloat(property, value); }
        if (parentAnim != null) { parentAnim.SetFloat(property, value); }
    }
    public void UpdateAnimatorBool(string property, bool value)
    {
        //if (bodyAnim != null) { bodyAnim.SetBool(property, value); }
        if (headAnim != null) { headAnim.SetBool(property, value); }
        if (armAnim != null) { armAnim.SetBool(property, value); }
        if (parentAnim != null) { parentAnim.SetBool(property, value); }
    }
    public void UpdateAnimatorSpeed(float speed)
    {
        //if (bodyAnim != null) { bodyAnim.speed = speed; }
        if (headAnim != null) { headAnim.speed = speed; }
        if (armAnim != null) { armAnim.speed = speed; }
        if (parentAnim != null) { parentAnim.speed = speed; }
    }

}