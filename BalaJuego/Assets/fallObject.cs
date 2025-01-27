using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallObject : MonoBehaviour
{
    Vector3 ogPos;
    Rigidbody2D rb2d;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ground")
        {
            transform.position = ogPos;
            rb2d.velocity = Vector2.zero;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        ogPos = transform.position;
        ServiceLocator.Instance.Get<ITimeManager>().subscribeToTimeChange(changeTimeMagnitude);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void changeTimeMagnitude(object sender,timeData data)
    {
        rb2d.velocity /= data.oldMagnitude;
        rb2d.velocity *= data.currentMagnitude;


        rb2d.gravityScale /= data.oldMagnitude;
        rb2d.gravityScale *= data.currentMagnitude;
    }
}
