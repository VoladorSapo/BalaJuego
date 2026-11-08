using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerMove player) : base(player)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //AnimatorExtensions.UpdateAnimatorBool(player.Animators, "isRunning", true);
        player.anim.SetBool("isRunning", true);
        //AnimatorExtensions.UpdateAnimatorBool(player.Animators, "isGround", true);
        player.anim.SetBool("isGround", true);
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
        float objectiveVelocity = player.MoveX * player.maxSpeed * player.timeMagnitude;
        float currentvelocity;
        if (tangent.x > 0.01f)
        {
            currentvelocity = player.calcVelocity.x / tangent.x;
        }
        else
        {
            currentvelocity = player.calcVelocity.x;
        }
        currentvelocity = Mathf.MoveTowards(currentvelocity, objectiveVelocity, useAccel * Time.fixedDeltaTime * player.timeMagnitude);
        player.calcVelocity.x = currentvelocity * tangent.x;
        player.calcVelocity.y = currentvelocity * tangent.y;

        rb2d.linearVelocity = player.calcVelocity;
    }
    public override void OnExit()
    {
        base.OnExit();
        musicManager.Instance.StopWalking();

    }
}
