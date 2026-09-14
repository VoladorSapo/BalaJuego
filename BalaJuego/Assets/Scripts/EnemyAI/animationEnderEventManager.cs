using System;
using UnityEngine;

public class animationEnderEventManager:MonoBehaviour
{
    string currentAnimation;
    Action currentAction;

    public void endAnim(AnimatorStateInfo stateInfo)
    {
        print("try end animation");

        if (stateInfo.IsName(currentAnimation))
        {
            print("END ANIMATION");
            currentAction?.Invoke();
            currentAction = null;
            currentAnimation = "NULL";
        }
    }
    public void setAnim(Action action, string animName)
    {
        currentAction = action;
        currentAnimation = animName;
    }
}