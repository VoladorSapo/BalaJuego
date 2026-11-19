using UnityEngine;

public class ThrowOutEquipmentController : MonoBehaviour
{
    Rigidbody2D rigidbody;
    [SerializeField] float throwForce = 10.0f;
    [SerializeField] Vector2 throwAngle = new Vector2(0, 1);
    Collider collider;
    private void Start()
    {
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody2D>();
        DestroyRespawnController respawnController = GetComponent<DestroyRespawnController>();
        if (respawnController != null)
        {
            respawnController.subscribeToRespawnAction(resetPhysics);
        }
    }
    public virtual void throwOut(CharacterInteractor owner)
    {
        Debug.Log("throOut");
        IProyectile proyectile = GetComponent<IProyectile>();
        if (proyectile != null)
        {
            proyectile.DeactivateMovement();
        }
        if (rigidbody != null)
        {
            Debug.Log("rigid");

            rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }
        rigidbody.AddForce(throwAngle.normalized * throwForce, ForceMode2D.Impulse);
    } 

    void resetPhysics()
    {
        if (rigidbody != null)
        {
            rigidbody.bodyType = RigidbodyType2D.Kinematic;
            rigidbody.linearVelocity = Vector2.zero;
        }
    }
}