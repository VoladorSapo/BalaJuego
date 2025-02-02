using UnityEngine;
public class EnemyParentDetector : ObjectParentDetector<EnemyController>
{
    public override void BecomeFirst(EnemyController obj)
    {

        foreach (var item in obj.GetComponentsInChildren<SpriteRenderer>())
        {
            item.color = new Color32(179, 42, 42, 255);
        }
   }
    public override void UnBecomeFirst(EnemyController obj)
    {
        foreach (var item in obj.GetComponentsInChildren<SpriteRenderer>())
        {
            item.color = new Color(1, 1, 1, 1);
        }
    }
}
