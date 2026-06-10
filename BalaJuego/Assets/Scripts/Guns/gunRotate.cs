using UnityEditor;
using UnityEngine;

public class gunRotate : MonoBehaviour
{
    [SerializeField] bool followMouse;
    [SerializeField] Transform fullCharacter;

    [Header("Animator")]
    public Animator bodyAnim;
    public Animator headAnim;
    public Animator armAnim;
    string currentStm = "MID";

    [SerializeField] Transform notTurn;

    bool shoulRotate;
    private void FixedUpdate()
    {
        
        if (followMouse && Time.timeScale > 0 && shoulRotate )
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            if (mousePos.y > 650 && currentStm != "UP")
            {
                ChangeHeadSprite("UP");
            }
            else if (mousePos.y < 200 && currentStm != "DOWN")
            {
                ChangeHeadSprite("DOWN");
            }
            else if (mousePos.y > 200 && mousePos.y < 650 && currentStm != "MID")
            {
                ChangeHeadSprite("MID");
            }
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
        if (bodyAnim != null) { bodyAnim.SetFloat(property, value); }
        if (headAnim != null) { headAnim.SetFloat(property, value); }
        if (armAnim != null) { armAnim.SetFloat(property, value); }
    }
    public void UpdateAnimatorBool(string property, bool value)
    {
        if (bodyAnim != null) { bodyAnim.SetBool(property, value); }
        if (headAnim != null) { headAnim.SetBool(property, value); }
        if(armAnim != null) { armAnim.SetBool(property, value); }
    }
    public void UpdateAnimatorSpeed(float speed)
    {
        if (bodyAnim != null) { bodyAnim.speed = speed; }
        if (headAnim != null) { headAnim.speed = speed; }
        if (armAnim != null) { armAnim.speed = speed; }
    }

    public void ChangeHeadSprite(string targetStm)
    {
        string[] stmNames = { "DOWN", "MID", "UP" };
        string[] knownStateNames = { "IDLE", "WALKB", "RUN", "JUMP", "FALL", "LAND" };
        if (headAnim != null) 
        {
            AnimatorStateInfo currentState = headAnim.GetCurrentAnimatorStateInfo(0);
            string currentShortName = GetShortNameFromHash(currentState.shortNameHash, knownStateNames);
            string targetFullStateName = $"{targetStm}.{currentShortName}";
            int targetHash = Animator.StringToHash(targetFullStateName);
            float normalizedTime = currentState.normalizedTime;

            currentStm = targetStm;
            headAnim.Play(targetHash, 0, normalizedTime);
        }
    }
    private string GetShortNameFromHash(int hash, string[] knownStateNames)
    {
        foreach (string name in knownStateNames)
        {
            if (Animator.StringToHash(name) == hash)
                return name;
        }
        return null;
    }

}