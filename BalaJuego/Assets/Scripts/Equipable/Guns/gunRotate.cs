using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class gunRotate : MonoBehaviour
{
    [SerializeField] bool followMouse;
    [SerializeField] Transform fullCharacter;

    [Header("Animator")]
    //[SerializeField] public Dictionary<string, Animator> animators;
    [SerializeField] Animator anim;
    //string currentStm = "MID";

    [SerializeField] Transform notTurn;

    bool gameStateRotate;
  [SerializeField] protected bool shouldRotate;

    [SerializeField] Transform backArm;
    [SerializeField] Transform frontArm;
    [field:SerializeField] public bool frontArmOn {  get; set; }
    private void FixedUpdate()
    {

        if (followMouse && Time.timeScale > 0 && gameStateRotate)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            //animators["head"].SetFloat("cursorY", mousePos.y);
            mousePos.z = 10;
            setRotation(Camera.main.ScreenToWorldPoint(mousePos));
        }
    }
    public virtual void setRotation(Vector3 obj)
    {
        Vector3 direction = obj - fullCharacter.position;

        Vector3 gunDirection = obj - (frontArmOn ? frontArm.position : backArm.position);
        float angle = Mathf.Round(Mathf.Atan2(gunDirection.y, Mathf.Abs(gunDirection.x)) * Mathf.Rad2Deg);
        // print(obj +" "+ fullCharacter.position + " "+direction+" "+angle);
        setRotationAngle(direction.x, angle);
    }
    protected void setRotationAngle(float direction, float angle)
    {
        if (direction > 0)
        {
            //AnimatorExtensions.UpdateAnimatorBool(animators,"direction", true);
            if (anim.HasParameter("direction")) anim.SetBool("direction", true);
            fullCharacter.localScale = new Vector3(-1, 1, 1);
            notTurn.eulerAngles = new Vector3(notTurn.eulerAngles.x, 180, notTurn.eulerAngles.z);

        }
        else if (direction < 0)
        {
            //AnimatorExtensions.UpdateAnimatorBool(animators, "direction", false);
            if (anim.HasParameter("direction")) anim.SetBool("direction", false);
            fullCharacter.localScale = new Vector3(1, 1, 1);
            notTurn.eulerAngles = new Vector3(notTurn.eulerAngles.x, 0, notTurn.eulerAngles.z);
        }
        Vector3 reference = direction > 0 ? Vector3.forward : Vector3.back;

        if (shouldRotate)
        {
            frontArm.rotation = frontArmOn ?  Quaternion.AngleAxis(angle, reference) : Quaternion.identity;
            backArm.rotation = !frontArmOn ? Quaternion.AngleAxis(angle, reference) : Quaternion.identity;
        }
        else
        {
            backArm.rotation = Quaternion.identity;
            frontArm.rotation = Quaternion.identity;
        }
    }
    protected virtual void Start()
    {
        ServiceLocator.Instance.Get<IGameState>().subscribeToStateChange(changeState);
        gameStateRotate = false;
    }
    void changeState(object sender, stateData data)
    {
        switch (data.currentState)
        {
            case IGameState.gameState.SlowDown:
                gameStateRotate = true;


                break;
            case IGameState.gameState.NormalTime:
                gameStateRotate = true;


                break;
            case IGameState.gameState.Tutorial:
                gameStateRotate = true;
                break;
            default:
                gameStateRotate = false;
                break;
        }
    }


}
