public class EnemyMoveState : BaseEnemyState
{

    AMove movement;

    public EnemyMoveState(AEnemyBehaviour _enemy)
    {
        enemy = _enemy;
        movement = enemy.GetComponent<AMove>();
    }

    public override void OnEnter()
    {
        movement.StartMovement();
    }
    public override void Update()
    {
        movement.UpdateMovement();
    }
    public override void OnExit()
    {


    }
}
public class EnemyShootMoveState : BaseEnemyState
{
    EnemyMoveState move;
    EnemyShootState shoot;
    public EnemyShootMoveState(AEnemyBehaviour _enemy,EnemyMoveState move,EnemyShootState shoot)
    {
        enemy = _enemy;
        this.move = move;
        this.shoot = shoot;
        
    }
    public override void OnEnter()
    {
        move.OnEnter();
        shoot.OnEnter();
    }
    public override void Update()
    {
        move.Update();
        shoot.Update();
    }
    public override void OnExit()
    {

        move.OnExit();
        shoot.OnExit();
    }
    public override void FixedUpdate()
    {
        move.FixedUpdate();
        shoot.FixedUpdate();
    }
}