using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Vector2 Move;

   [SerializeField] Vector2 calcVelocity;

   [SerializeField] BoxCollider2D groundCast;
    [SerializeField] private Rigidbody2D rb2d;

    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float groundDecceleration;
    [SerializeField] float turnDecceleration;

    [SerializeField] float airDecceleration;
    [SerializeField] float jumpForce;

    [SerializeField] float normalGravity;
    [SerializeField] float fallGravity;


    [SerializeField] LayerMask groudLayers;


 [SerializeField]   bool onGround;
    [SerializeField] bool jumpPressed;


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit2D hit = Physics2D.BoxCast(groundCast.transform.position, groundCast.size, 0, Vector2.right,groudLayers);
        Vector3 start = new Vector3(groundCast.transform.position.x - groundCast.size.x / 2, groundCast.transform.position.y - groundCast.size.y / 2, 0);
        Vector3 end = new Vector3(groundCast.transform.position.x + groundCast.size.x / 2, groundCast.transform.position.y - groundCast.size.y / 2, 0);
        onGround = hit;
        if (onGround)
        {
            rb2d.gravityScale = normalGravity;

        }
        Debug.DrawLine(start, end);

        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            rb2d.AddForce(Vector2.up * jumpForce);
        }
        if (!onGround && (Input.GetKeyUp(KeyCode.Space) || rb2d.velocity.y <0))
        {
            rb2d.gravityScale = fallGravity;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ServiceLocator.Instance.Get<ITimeManager>().changeTimeMagnitude(0.2f);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            ServiceLocator.Instance.Get<ITimeManager>().changeTimeMagnitude(1);
        }
    }
    private void FixedUpdate()
    {
        Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        calcVelocity = rb2d.velocity;
     
        if (Move.x == 0)
        {
            calcVelocity.x = Mathf.MoveTowards(calcVelocity.x,0, groundDecceleration * Time.fixedDeltaTime);

        }
        else {
            float useAccel = (Mathf.Abs(calcVelocity.x)==0 || Mathf.Sign(calcVelocity.x) == Move.x) ? acceleration : turnDecceleration;
           
            calcVelocity.x = Mathf.MoveTowards(calcVelocity.x, Move.x * maxSpeed, useAccel * Time.fixedDeltaTime);
        }
        rb2d.velocity = calcVelocity;

    }
}
