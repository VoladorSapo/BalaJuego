using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerMove : MonoBehaviour
{
    Vector2 Move;

    [SerializeField] float trueMagnitude;

    [SerializeField] Vector2 calcVelocity;

    [SerializeField] BoxCollider2D groundCast;
    [SerializeField] private Rigidbody2D rb2d;

    [SerializeField]public float maxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float groundDecceleration;
    [SerializeField] float turnDecceleration;

    [SerializeField] float airDecceleration;
    [SerializeField] float jumpForce;

    [SerializeField] float normalGravity;
    [SerializeField] float fallGravity;


    [SerializeField] LayerMask groudLayers;

public bool onGround;
    [SerializeField] bool jumpPressed;


    [SerializeField] float coyoteTime;
    [SerializeField] float jumpBufferTime;

    [SerializeField] bool jumping, falling;

  public  bool isMeleeing;

    float coyoteTimeCurrent;
    float jumpBufferTimeCurrent;

    [SerializeField] float maxFallVelocity;

    PlayerShoot shoot;

 public   Animator anim;

    float timeMagnitude;

    IShoot gun;
 public   int runningDirection{get;private set;}    

    Vector3 initialPos;

    [SerializeField] GameObject particles;
    ParticleSystem dustWalk, dustJump,
        dustFall;
    bool isRotating;

    bool canMove;
    private void Awake()
    {
        initialPos = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        timeMagnitude = 1;
        isMeleeing = false;
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponentsInChildren<Animator>()[0];
        dustWalk = GetComponentsInChildren<ParticleSystem>()[0];
        dustJump = GetComponentsInChildren<ParticleSystem>()[1];
        dustFall = GetComponentsInChildren<ParticleSystem>()[2];
        shoot = GetComponent<PlayerShoot>();
        dustWalk.gameObject.SetActive(false);
        dustJump.gameObject.SetActive(false);
        dustFall.gameObject.SetActive(false);
        ServiceLocator.Instance.Get<ILevelController>().subscribeToRestart(restart);
        ServiceLocator.Instance.Get<IGameState>().subscribeToStateChange(changeState);
        ServiceLocator.Instance.Get<ITimeManager>().subscribeToTimeChange(changeTimeMagnitude);

    }
    public void changeSpawnPoint(Vector3 pos)
    {
        initialPos = pos;
    }
    // Update is called once per frame
    void Update()
    {

        RaycastHit2D hit = Physics2D.BoxCast(groundCast.transform.position, groundCast.size, 0, Vector2.down, groundCast.size.y / 2, groudLayers);
        Vector3 start = new Vector3(groundCast.transform.position.x - groundCast.size.x / 2, groundCast.transform.position.y - groundCast.size.y / 2, 0);
        Vector3 end = new Vector3(groundCast.transform.position.x + groundCast.size.x / 2, groundCast.transform.position.y - groundCast.size.y / 2, 0);
        if(hit && !onGround)
        {
            dustFall.gameObject.SetActive(true);
            dustFall.Play();

            musicManager.Instance.PlaySoundPitch("snd_aterriza",0.2f);
        }
        onGround = hit;
        if (!onGround)
            dustWalk.Stop();
        anim.SetBool("isGround", onGround);
      
            shoot.stunedDetector.gameObject.SetActive(onGround);
        
        if (onGround && !jumping/* && rb2d.velocity.y <= 0*/)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            rb2d.gravityScale = 0;
            coyoteTimeCurrent = coyoteTime;

        }
        coyoteTimeCurrent -= Time.deltaTime;
        jumpBufferTimeCurrent -= Time.deltaTime;
        Debug.DrawLine(start, end);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferTimeCurrent = jumpBufferTime;
        }
        if (!onGround && (Input.GetKeyUp(KeyCode.Space) || rb2d.velocity.y >= 0))
        {
            rb2d.gravityScale = normalGravity;
        }
        if (!onGround && (Input.GetKeyUp(KeyCode.Space) || rb2d.velocity.y < 0))
        {
            rb2d.gravityScale = fallGravity;
        }
        if (rb2d.velocity.y < 0)
        {
            falling = true;
        }
        if (onGround && falling)
        {
            //ustJump.gameObject.SetActive(true);
            //dustJump.Play();
            jumping = false;
            falling = false;
        }



    }
    private void FixedUpdate()
    {
        if (canMove && !isMeleeing)
        {
            Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            calcVelocity = rb2d.velocity;

            if (Move.x == 0)
            {
                calcVelocity.x = Mathf.MoveTowards(calcVelocity.x, 0, groundDecceleration * Time.fixedDeltaTime);
                anim.SetBool("isRunning", false);
                musicManager.Instance.StopWalking();

                dustWalk.Stop();

            }
            else
            {
                anim.SetBool("isRunning", true);
                if (onGround) { musicManager.Instance.StartWalking(); /*Debug.Log("PASOOOOOOOOOOOOOOOOOOO");*/ } else { musicManager.Instance.StopWalking(); }

                dustWalk.gameObject.SetActive(true);
                if (onGround) dustWalk.Play();
                float useAccel = (Mathf.Abs(calcVelocity.x) == 0 || Mathf.Sign(calcVelocity.x) == Move.x) ? acceleration : turnDecceleration;

                calcVelocity.x = Mathf.MoveTowards(calcVelocity.x, Move.x * maxSpeed * timeMagnitude, useAccel * Time.fixedDeltaTime * timeMagnitude);
            }
            anim.SetFloat("velocity", calcVelocity.x);
            if (calcVelocity.x > 0 && !isRotating && runningDirection != 1)
            {
                //dustWalk.transform.eulerAngles = new Vector3(0, 180, 0);
                StartCoroutine("rotateParticle", true);
                runningDirection = 1;
            }
            else if (calcVelocity.x < 0 && !isRotating && runningDirection != -1)
            {
                StartCoroutine("rotateParticle", false);
                runningDirection = -1;
                //dustWalk.transform.eulerAngles = new Vector3(0,0, 0);
            }

            anim.SetFloat("verticalVelocity", calcVelocity.y);
            if (calcVelocity.y < -maxFallVelocity)
            {
                calcVelocity.y = -maxFallVelocity;
            }
            rb2d.velocity = calcVelocity;
            if (coyoteTimeCurrent > 0 && jumpBufferTimeCurrent > 0 && !jumping)
            {
                dustJump.gameObject.SetActive(true);
                dustJump.Play();
                musicManager.Instance.PlayJump();

                jumping = true;
                rb2d.gravityScale = normalGravity;
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                rb2d.AddForce(Vector2.up * jumpForce , ForceMode2D.Impulse);
                coyoteTimeCurrent = jumpBufferTimeCurrent = 0;
            }
        }
        else
        {
            musicManager.Instance.StopWalking();
        }

    }


    public IEnumerator rotateParticle(bool direction)
    {
        isRotating = true;
        if (direction)
        {
            for (int i = 0; i < 10; i++)
            {
                yield return (new WaitForSeconds(.5f / 10));
                dustWalk.transform.eulerAngles += new Vector3(0, 18, 0);
            }
            dustWalk.transform.eulerAngles = new Vector3(0, 180, 0);
            isRotating = false;
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                yield return (new WaitForSeconds(.5f / 10));
                dustWalk.transform.eulerAngles -= new Vector3(0, 18, 0);
            }
            dustWalk.transform.eulerAngles = new Vector3(0, 0, 0);
            isRotating = false;
        }


    }
    public void restart()
    {
        print("plyerRestart");
        transform.position = initialPos;
        gameObject.SetActive(true);
        GetComponentInChildren<IShoot>().restart();
        GetComponentInChildren<PlayerShoot>().restart();
        GetComponent<CharacterLife>().restart();
        canMove = true;
        isMeleeing = false;

    }
    void changeState(object sender, stateData data)
    {
        switch (data.currentState)
        {
            case IGameState.gameState.NormalTime:
                canMove = true;


                break;
            case IGameState.gameState.SlowDown:
                canMove = true;

                break;
            default:
                canMove = false;
                rb2d.velocity = new Vector2(0,rb2d.velocity.y);
                anim.SetBool("isRunning", false);

                break;
           
            
        }
    }
    void changeTimeMagnitude(object sender, timeData data)
    {
        float use = data.currentMagnitude == 1 ? 1 : trueMagnitude;
        float mult = data.currentMagnitude / data.oldMagnitude;
        timeMagnitude = use;

        if (use == 1)
        {
            Vector2 vel = rb2d.velocity / new Vector2(trueMagnitude, 1);
            vel = new Vector2(Mathf.Clamp(vel.x, 0, 999), Mathf.Clamp(vel.y, 0, 999));
            if(float.IsNaN(vel.x))
            {
                print("hola");
                vel.x = 0;
            }
            print(vel.x);
            rb2d.velocity = vel;
        }
        else
        {
            rb2d.velocity *= new Vector2(trueMagnitude, 1);

        }
        anim.speed = use;

        //maxSpeed*=mult;
        //acceleration*=mult;
        //groundDecceleration*=mult;
        //turnDecceleration*=mult;

        //airDecceleration*=mult;
        //jumpForce*=mult;
        if (data.currentMagnitude != 1)
        {
            normalGravity = mult=1.5f;
            fallGravity = mult=2;
            jumpForce = 5;
        }
        else
        {
            normalGravity = mult = 4;
            fallGravity = mult = 8;
            jumpForce = 8;
        }
    }

}

