using UnityEngine;

public abstract class PlayerBaseState: IState
{
    protected PlayerMove player;
    protected Rigidbody2D rb2d;
    public PlayerBaseState(PlayerMove player)
    {
        this.player = player;
    }

    public virtual void OnEnter()
    {
        rb2d = player.GetComponent<Rigidbody2D>();
    }
    public virtual void Update()
    {
        Debug.Log("MOveX" + (int)player.playerInput.Move);
        player.MoveX = (int)player.playerInput.Move;
    }
    public virtual void FixedUpdate()
    {
        player.calcVelocity = rb2d.velocity;
    }
    public virtual void OnExit()
    {

    }

    public virtual bool ShouldEnd()
    {
        return false;
    }
}