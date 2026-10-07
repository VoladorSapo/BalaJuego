using UnityEngine;

public class AnimatorHover : ABaseHover
{
    [SerializeField] Animator anim;
    [SerializeField] string hoverState;
    [SerializeField] string ogState;


    private void Start()
    {
    }
    public override void setHover(bool set)
    {
        anim.Play(set ? hoverState : ogState);
    }
}