using UnityEngine;
public class EnemyParentDetector : ObjectParentDetector<EnemyBehaviour>
{
    public override void BecomeFirst(EnemyBehaviour obj)
    {

        obj.setColor(true);
   }
    public override void UnBecomeFirst(EnemyBehaviour obj)
    {
        obj.setColor(false);

    }
}
