using UnityEngine;

public class LinearFlyMove : AEnemyMove
{

    public override void UpdateMovement()
    {
        charTransform.position = Vector3.MoveTowards(charTransform.position, currentObjectivePos, speed * Time.deltaTime * timeMagnitude);
        
    }
 
    public override void StartMovement()
    {
    
    }

    protected override void Start()
    {
        base.Start();
        startPosition = charTransform.position;
    }

   
}
