using UnityEngine;

public class PlayerExecuteState : PlayerBaseState
{
    public PlayerExecuteState(PlayerMove player) : base(player)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        player.playerLife.setInvincibility(true);
        player.calcVelocity = Vector2.zero;
        rb2d.gravityScale = 0;
        rb2d.linearVelocity = player.calcVelocity;
        foreach(GameObject o in player.playerShoot.hidewhenMelee)
        {
            o.SetActive(false);
        }
    }
    public override void OnExit()
    {
        base.OnExit();

        player.playerLife.setInvincibility(false);
        foreach (GameObject o in player.playerShoot.hidewhenMelee)
        {
            o.SetActive(true);
        }
       player.GetComponentInChildren<IGun>().getAnim().Play("gunReload");

    }
}