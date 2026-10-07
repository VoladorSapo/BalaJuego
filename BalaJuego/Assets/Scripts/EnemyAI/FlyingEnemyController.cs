public class FlyingEnemyController : GunEnemyBehaviour
{
    public override void setUpStateMachine()
    {
        stateMachine = new StateMachine();
        EnemyMoveState move = new EnemyMoveState(this);
        EnemyShootState shoot = new EnemyShootState(this);

        EnemyStunedState stuned = new EnemyStunedState(this);
        EnemyShootMoveState shootmove = new EnemyShootMoveState(this,move,shoot);

        stateMachine.AddTransition(move, shootmove, new FuncPredicate(() => detectorManager.detectorDictionary["in"].getCount() > 0));
        stateMachine.AddTransition(shootmove, move, new FuncPredicate(() => detectorManager.detectorDictionary["out"].getCount() == 0));
        stateMachine.AddAnyTransition(stuned, new FuncPredicate(() => Charshoot.getBullets() == 0 && canBeKilledMelee));
        stateMachine.setDefaultState(move);
    }
}

