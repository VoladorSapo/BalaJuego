using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerMove player) : base(player)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.UpdateAnimatorBool("isRunning", true);
        player.UpdateAnimatorBool("isGround", true);
        musicManager.Instance.StartWalking();
    }
    public override void Update()
    {
        base.Update();
        player.beOnGround();

    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        //dustWalk.gameObject.SetActive(true);
        //if (onGround) dustWalk.Play();
        //CALCULAR UNA SOLA VELOCIDAD OVJETIVO SIN TENER EN CUENTA EL SLOPE. LUEGO PARTIRLA EN DOS EJES
        float useAccel = (Mathf.Abs(player.calcVelocity.x) == 0 || Mathf.Sign(player.calcVelocity.x) == player.MoveX) ?   player.acceleration : player.turnDecceleration;
        Vector2 tangent = new Vector2(player.hitGround.normal.y, -player.hitGround.normal.x);
        player.calcVelocity.x = Mathf.MoveTowards(player.calcVelocity.x, tangent.x* player.MoveX * player.maxSpeed * player.timeMagnitude, useAccel * Time.fixedDeltaTime * player.timeMagnitude);
        player.calcVelocity.y = Mathf.MoveTowards(player.calcVelocity.y, tangent.y * player.MoveX * player.maxSpeed * player.timeMagnitude, useAccel * Time.fixedDeltaTime * player.timeMagnitude);

        rb2d.linearVelocity = player.calcVelocity;
    }
    public override void OnExit()
    {
        base.OnExit();
        musicManager.Instance.StopWalking();

    }
}
