using System;
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
        //Debug.Log("MOveX" + (int)player.playerInput.Move);
        player.MoveX = (int)player.playerInput.Move;
    }
    public virtual void FixedUpdate()
    {
        player.calcVelocity = rb2d.linearVelocity;
    }
    public virtual void OnExit()
    {

    }

    public virtual bool ShouldEnd()
    {
        return false;
    }

    public void SetUp(string Data)
    {
    }

    public virtual bool hasPreExitAction()
    {
        return false;
    }

    public  virtual void preExit(Action action)
    {
    }
}