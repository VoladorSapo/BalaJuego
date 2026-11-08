using UnityEngine;

public class AgathaHeadSpriteController : AHeadSpriteController
{
    [Header("Mouse LookingAt")]
    [SerializeField] float threshold = 25f;
    [SerializeField] float hysteresis = 6f;
    [SerializeField] Transform flipOrigin;

    [Header("Gizmo")]
    [SerializeField] float longitudGizmo = 2f;
    float RelAngle;
    Camera cam;

    void LateUpdate()
    {
        if (cam == null) cam = Camera.main;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(cam.transform.position.z - transform.position.z);
        Vector2 target = cam.ScreenToWorldPoint(mousePos);

        Vector2 dir = target - (Vector2)transform.position;

        float side = -Side();

        RelAngle = Mathf.Atan2(dir.y, dir.x * side) * Mathf.Rad2Deg;

        lookingAt = CheckAngle(RelAngle);
        base.LateUpdate();

    }

    HeadLookingState CheckAngle(float a)
    {
        float t = lookingAt == HeadLookingState.mid ? threshold : threshold - hysteresis;
        if (a > t) return HeadLookingState.up;
        if (a < -t) return HeadLookingState.down;
        return HeadLookingState.mid;
    }

    void OnDrawGizmos()
    {
        if (transform == null || flipOrigin == null) return;

        Vector3 o = transform.position;
        float lado = -Side();

        // Convierte un ángulo relativo (0 = delante) a un vector de mundo, aplicando el flip
        Vector3 Dir(float grados)
        {
            float r = grados * Mathf.Deg2Rad;
            return new Vector3(Mathf.Cos(r) * lado, Mathf.Sin(r), 0f);
        }

        // Delante (0°)
        Gizmos.color = Color.white;
        Gizmos.DrawLine(o, o + Dir(0f) * longitudGizmo);

        // Límites de entrada a Arriba / Abajo (umbral)
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(o, o + Dir(threshold) * longitudGizmo);
        Gizmos.DrawLine(o, o + Dir(-threshold) * longitudGizmo);

        // Límites de salida (umbral - histéresis), más tenues
        Gizmos.color = new Color(1f, 0.6f, 0f, 0.6f);
        Gizmos.DrawLine(o, o + Dir(threshold - hysteresis) * longitudGizmo * 0.8f);
        Gizmos.DrawLine(o, o + Dir(-(threshold - hysteresis)) * longitudGizmo * 0.8f);

        // Dirección real hacia el ratón (solo en Play)
        if (Application.isPlaying)
        {
            Gizmos.color = lookingAt switch
            {
                HeadLookingState.up => Color.green,
                HeadLookingState.down => Color.red,
                _ => Color.cyan
            };
            Gizmos.DrawLine(o, o + Dir(RelAngle) * longitudGizmo * 1.2f);
            Gizmos.DrawWireSphere(o + Dir(RelAngle) * longitudGizmo * 1.2f, 0.06f);
        }
    }

    float Side()
    {
        float scale = flipOrigin.lossyScale.x >= 0 ? 1f : -1f;
        float rot = flipOrigin.right.x >= 0 ? 1f : -1f;
        return scale * rot;
    }
}
