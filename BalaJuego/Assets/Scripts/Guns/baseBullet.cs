using UnityEngine;

public class baseBullet: MonoBehaviour, IBullet
{
    Vector3 direction;
    [SerializeField] float speed;
    [SerializeField] int damage = 1;
    [SerializeField] bool grabable = true;
    private void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
    public void InstantiateBullet(GameObject shooter,float angle)
    {
        print(shooter.transform.localScale.x);
        angle *= shooter.transform.localScale.x;
        transform.eulerAngles = new Vector3(0, shooter.transform.localScale.x < 0 ? -180 : 0, angle); 
    }

    public int getDamage() => damage;
}