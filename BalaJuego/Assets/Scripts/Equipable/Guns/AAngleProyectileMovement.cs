using System;
using UnityEngine;
using UnityEngine.Events;
public abstract class AAngleProyectileMovement: AProyectileMovement
{
   protected float angle;
    public override void ActivateProyectileMovement(CharacterInteractor shooter, Vector3 startPos, float angle)
    {
        ActivateProyectile(shooter, startPos);
        angle *= shooter.transform.localScale.x;
        this.angle = angle;
        transform.eulerAngles = new Vector3(0, shooter.transform.localScale.x < 0 ? -180 : 0, angle);
    }

    public override void ActivateProyectileMovement(CharacterInteractor shooter, Vector3 startPos, Vector3 objective)
    {
        ActivateProyectileMovement(shooter, startPos, Vector2.Angle(startPos, objective));
    }

    public override void ActivateProyectileMovement(CharacterInteractor shooter, Vector3 startPos, Transform transform)
    {
        ActivateProyectileMovement(shooter, startPos, transform.position);
    }
}
