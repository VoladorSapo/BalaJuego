using UnityEngine;

public class EndEventAnimationBehaviour : StateMachineBehaviour
{
    protected bool finished;
    animationEnderEventManager endEvent;

    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        endEvent = animator.gameObject.GetComponentInParent<animationEnderEventManager>();
        finished = false;
    }

    public override void OnStateUpdate(UnityEngine.Animator animator, UnityEngine.AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (animatorStateInfo.normalizedTime > 0.9f && !finished)
        {
            finished = true;
            endEvent.endAnim(animatorStateInfo);
        }
    }
}
 