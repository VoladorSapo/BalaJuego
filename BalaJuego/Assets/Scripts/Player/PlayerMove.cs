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


 [SerializeField]   float coyoteTime;
    [SerializeField] float jumpBufferTime;

    [SerializeField] bool jumping,falling;

    float coyoteTimeCurrent;
    float jumpBufferTimeCurrent;

 [SerializeField]   float maxFallVelocity;

   Animator anim;

    IShoot gun;


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponentsInChildren<Animator>()[0];
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit2D hit = Physics2D.BoxCast(groundCast.transform.position, groundCast.size, 0,Vector2.right, groundCast.size.y/2, groudLayers);
        Vector3 start = new Vector3(groundCast.transform.position.x - groundCast.size.x / 2, groundCast.transform.position.y - groundCast.size.y / 2, 0);
        Vector3 end = new Vector3(groundCast.transform.position.x + groundCast.size.x / 2, groundCast.transform.position.y - groundCast.size.y / 2, 0);
        onGround = hit;
        anim.SetBool("isGround", onGround);
        if (onGround && !jumping)
        {
            rb2d.gravityScale = normalGravity;
            coyoteTimeCurrent = coyoteTime;

        }
        coyoteTimeCurrent-=Time.deltaTime;
        jumpBufferTimeCurrent -= Time.deltaTime;
        Debug.DrawLine(start, end);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferTimeCurrent = jumpBufferTime;
        }
        if (!onGround && (Input.GetKeyUp(KeyCode.Space) || rb2d.velocity.y <0))
        {
            rb2d.gravityScale = fallGravity;
        }
        if (rb2d.velocity.y < 0)
        {
            falling = true;
        }
        if(onGround && falling)
        {
            jumping = false;
            falling = false;

        }
       
        
     
    }
    private void FixedUpdate()
    {
        Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        calcVelocity = rb2d.velocity;
     
        if (Move.x == 0)
        {
            calcVelocity.x = Mathf.MoveTowards(calcVelocity.x,0, groundDecceleration * Time.fixedDeltaTime);
            anim.SetBool("isRunning", false);

        }
        else {
            anim.SetBool("isRunning", true);

            float useAccel = (Mathf.Abs(calcVelocity.x)==0 || Mathf.Sign(calcVelocity.x) == Move.x) ? acceleration : turnDecceleration;
           
            calcVelocity.x = Mathf.MoveTowards(calcVelocity.x, Move.x * maxSpeed, useAccel * Time.fixedDeltaTime);
        }
        anim.SetFloat("velocity", calcVelocity.x);
        anim.SetFloat("verticalVelocity", calcVelocity.y);
        if(calcVelocity.y < -maxFallVelocity)
        {
            calcVelocity.y = -maxFallVelocity;
        }
        rb2d.velocity = calcVelocity;
        if (coyoteTimeCurrent > 0 && jumpBufferTimeCurrent > 0 && !jumping)
        {
            jumping = true;
            print("jump");
            rb2d.gravityScale = normalGravity;
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.AddForce(Vector2.up * jumpForce,ForceMode2D.Impulse);
            coyoteTimeCurrent = jumpBufferTimeCurrent = 0;
        }

    }
}
