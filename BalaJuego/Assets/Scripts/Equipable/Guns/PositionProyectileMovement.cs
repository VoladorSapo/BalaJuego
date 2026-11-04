using UnityEngine;

public abstract class PositionProyectileMovement : AProyectileMovement
{
    [SerializeField] AnimationCurve curve;
    [SerializeField] float maxRange;
    Vector3 startPos;
    Vector3 endPos;
    public override void ActivateProyectileMovement(CharacterInteractor shooter, Vector3 startPos, Vector3 objective)
    {
        this.startPos = startPos;
        this.endPos = objective;
    }
    public override void ActivateProyectileMovement(CharacterInteractor shooter, Vector3 startPos, Transform transform)
    {
        ActivateProyectileMovement(shooter,startPos,transform.position);
    }
    public override void ActivateProyectileMovement(CharacterInteractor shooter, Vector3 startPos, float angle)
    {
        Vector3 dir = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));
        ActivateProyectileMovement(shooter, startPos, startPos + dir * maxRange);


    }
}