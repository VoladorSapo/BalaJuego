using UnityEngine;
public class EnemyDetector : ObjectDetector<AEnemyBehaviour>
{
    public override void BecomeFirst(AEnemyBehaviour obj)
    {

        obj.setColor(true);
   }
    public override void UnBecomeFirst(AEnemyBehaviour obj)
    {
        obj.setColor(false);

    }
}
