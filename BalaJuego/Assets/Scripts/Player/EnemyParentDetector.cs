using UnityEngine;
public class EnemyParentDetector : ObjectParentDetector<AEnemyBehaviour>
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
