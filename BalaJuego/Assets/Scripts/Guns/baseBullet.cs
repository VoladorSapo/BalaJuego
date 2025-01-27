using UnityEngine;

public class baseBullet: MonoBehaviour, IBullet
{
    Vector3 direction;
    [SerializeField] float speed;
    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
    public void InstantiateBullet(Vector3 _direction)
    {
        direction = _direction;
    }
}