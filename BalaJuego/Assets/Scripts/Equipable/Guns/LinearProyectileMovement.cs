using UnityEngine;

public class LinearProyectileMovement :AAngleProyectileMovement{

    protected override void Move()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime * timeMagnitude);
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }
    }
