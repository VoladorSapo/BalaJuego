using UnityEngine;
public class EnemyParentDetector : ObjectParentDetector<EnemyController>
{
    public override void BecomeFirst(EnemyController obj)
    {

        obj.setColor(true);
   }
    public override void UnBecomeFirst(EnemyController obj)
    {
        obj.setColor(false);

    }
}
