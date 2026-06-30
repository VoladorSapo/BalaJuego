using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerMove : MonoBehaviour
{
    public int MoveX;

    [SerializeField] float trueMagnitude;

    [field: SerializeField] public Vector2 calcVelocity;

    [SerializeField] BoxCollider2D groundCast;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] public PlayerInput playerInput;

    [field: SerializeField] public float maxSpeed { get; private set; }
    [field: SerializeField] public float acceleration { get; private set; }
    [field: SerializeField] public float groundDecceleration {  get; private set; }
    [field: SerializeField] public float turnDecceleration { get; private set; }
    [field: SerializeField] public float airDecceleration { get; private set; }
    [field: SerializeField] public float jumpForce { get; private set; }

    [field: SerializeField] public float normalGravity { get; private set; }
    [field: SerializeField] public float fallGravity { get; private set; }


    [SerializeField] LayerMask groudLayers;

    public bool onGround;
    [SerializeField] bool jumpPressed;


    [field: SerializeField] public float coyoteTime { get; private set; }

    [field: SerializeField] public float jumpBufferTime { get; private set; }

    [SerializeField] bool jumping, falling;

    public bool isMeleeing;

  public  float coyoteTimeCurrent { get; set; }
  public  float jumpBufferTimeCurrent { get; set; }
    [field:SerializeField] public float maxFallVelocity { get;private set; }
    [SerializeField] GameObject Head;
    [SerializeField] GameObject gunOBJ;
    PlayerShoot shoot;

    [Header("Animator")]
    //public Animator bodyAnim;
    public Animator headAnim;
    public Animator armAnim;
    public Animator parentAnim;

   public float timeMagnitude { get; private set; }

    IGun gun;
    public int runningDirection { get; private set; }

    Vector3 initialPos;

    [SerializeField] GameObject particles;

    StateMachine playerMoveStateMachine;
    //ParticleSystem dustWalk, dustJump,
    //    dustFall;
    bool isRotating;

    bool canMove;

    RaycastHit2D hitGround;


    private void Awake()
    {
        initialPos = transform.position;
    }
    void setUpMoveStateMachine()
    {
        playerMoveStateMachine = new StateMachine();
        PlayerIdleState idle = new PlayerIdleState(this);
        PlayerWalkState walk = new PlayerWalkState(this);
        PlayerJumpState jump = new PlayerJumpState(this);
        PlayerFallState fall = new PlayerFallState(this);


        FuncPredicate canJump = new FuncPredicate(() => coyoteTimeCurrent > 0 && jumpBufferTimeCurrent > 0);
        FuncPredicate onGround = new FuncPredicate(() => {  return hitGround; });
        FuncPredicate notOnGround = new FuncPredicate(() => {  return !hitGround; });


        playerMoveStateMachine.AddTransition(idle, walk, new FuncPredicate(() => MoveX != 0));
        playerMoveStateMachine.AddTransition(walk, idle, new FuncPredicate(() => MoveX == 0));
        
        playerMoveStateMachine.AddTransition(idle, jump, canJump);
        playerMoveStateMachine.AddTransition(walk, jump, canJump);
        playerMoveStateMachine.AddTransition(jump, fall, new FuncPredicate(() =>rb2d.velocity.y<0));

        playerMoveStateMachine.AddTransition(idle, fall, notOnGround);
        playerMoveStateMachine.AddTransition(walk, fall, notOnGround);
        playerMoveStateMachine.AddTransition(fall, idle, onGround);


        playerMoveStateMachine.SetState(idle);

    }
    // Start is called before the first frame update
    void Start()
    {
        setUpMoveStateMachine();
        timeMagnitude = 1;
        isMeleeing = false;
        rb2d = GetComponent<Rigidbody2D>();
        playerInput=GetComponent<PlayerInput>();
        //dustWalk = GetComponentsInChildren<ParticleSystem>()[0];
        //dustJump = GetComponentsInChildren<ParticleSystem>()[1];
        //dustFall = GetComponentsInChildren<ParticleSystem>()[2];
        shoot = GetComponent<PlayerShoot>();
        //dustWalk.gameObject.SetActive(false);
        //dustJump.gameObject.SetActive(false);
        //dustFall.gameObject.SetActive(false);
        ServiceLocator.Instance.Get<ILevelController>().subscribeToRestart(restart);
        ServiceLocator.Instance.Get<IGameState>().subscribeToStateChange(changeState);
        ServiceLocator.Instance.Get<ITimeManager>().subscribeToTimeChange(changeTimeMagnitude);

    }
    public void changeSpawnPoint(Vector3 pos)
    {
        initialPos = pos;
    }
    public void beOnGround()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        rb2d.gravityScale = 0;
        coyoteTimeCurrent = coyoteTime;
        UpdateAnimatorBool("isGround", true);
    }
    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (playerInput.JumpDown)
            {
                jumpBufferTimeCurrent = jumpBufferTime;
            }
            hitGround = Physics2D.BoxCast(groundCast.transform.position, groundCast.size, 0, Vector2.down, groundCast.size.y / 4, groudLayers);
            playerMoveStateMachine.Update();
            print(playerMoveStateMachine.currentState());
            coyoteTimeCurrent -= Time.deltaTime * timeMagnitude;
            jumpBufferTimeCurrent -= Time.deltaTime * timeMagnitude;
        }
        //Vector3 start = new Vector3(groundCast.transform.position.x - groundCast.size.x / 2, groundCast.transform.position.y - groundCast.size.y / 2, 0);
        //Vector3 end = new Vector3(groundCast.transform.position.x + groundCast.size.x / 2, groundCast.transform.position.y - groundCast.size.y / 2, 0);
        //if (hit && !onGround)
        //{
        //    //dustFall.gameObject.SetActive(true);
        //    //dustFall.Play();

        //    musicManager.Instance.PlaySoundPitch("snd_aterriza", 0.2f);
        //}
        //onGround = hit;
        //if (!onGround) { }
        ////    //dustWalk.Stop();
        //UpdateAnimatorBool("isGround", onGround);

        //shoot.stunedDetector.gameObject.SetActive(onGround);

        //if (onGround && !jumping/* && rb2d.velocity.y <= 0*/)
        //{
        //    rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        //    rb2d.gravityScale = 0;
        //    coyoteTimeCurrent = coyoteTime;

        //}
        //coyoteTimeCurrent -= Time.deltaTime;
        //jumpBufferTimeCurrent -= Time.deltaTime;
        //Debug.DrawLine(start, end);

        //if (playerInput.JumpDown)
        //{
        //    jumpBufferTimeCurrent = jumpBufferTime;
        //}
        //if (!onGround && (playerInput.JumpUp || rb2d.velocity.y >= 0))
        //{
        //    rb2d.gravityScale = normalGravity;
        //}
        //if (!onGround && (playerInput.JumpUp || rb2d.velocity.y < 0))
        //{
        //    rb2d.gravityScale = fallGravity;
        //}
        //if (rb2d.velocity.y < 0)
        //{
        //    falling = true;
        //}
        //if (onGround && falling)
        //{
        //    //ustJump.gameObject.SetActive(true);
        //    //dustJump.Play();
        //    jumping = false;
        //    falling = false;
        //}



    }
    private void FixedUpdate()
    {
        if (canMove)
        {
            playerMoveStateMachine.FixedUpdate();
            UpdateAnimatorFloat("velocity", calcVelocity.x);

            UpdateAnimatorFloat("verticalVelocity", calcVelocity.y);
        }

            //if (canMove && !isMeleeing)
            //{
            //    MoveX = (int)playerInput.Move;

            //    calcVelocity = rb2d.velocity;

            //    if (MoveX == 0)
            //    {

            //        //dustWalk.Stop();

            //    }
            //    else
            //    {
            //        UpdateAnimatorBool("isRunning", true);
            //        if (onGround) { musicManager.Instance.StartWalking(); /*Debug.Log("PASOOOOOOOOOOOOOOOOOOO");*/ } else { musicManager.Instance.StopWalking(); }

            //        //dustWalk.gameObject.SetActive(true);
            //        //if (onGround) dustWalk.Play();
            //        float useAccel = (Mathf.Abs(calcVelocity.x) == 0 || Mathf.Sign(calcVelocity.x) == MoveX) ? acceleration : turnDecceleration;

            //        calcVelocity.x = Mathf.MoveTowards(calcVelocity.x, MoveX * maxSpeed * timeMagnitude, useAccel * Time.fixedDeltaTime * timeMagnitude);
            //    }
        //    if (calcVelocity.x > 0 && !isRotating && runningDirection != 1)
        //    {
        //        //dustWalk.transform.eulerAngles = new Vector3(0, 180, 0);
        //        StartCoroutine("rotateParticle", true);
        //        runningDirection = 1;
        //    }
        //    else if (calcVelocity.x < 0 && !isRotating && runningDirection != -1)
        //    {
        //        StartCoroutine("rotateParticle", false);
        //        runningDirection = -1;
        //        //dustWalk.transform.eulerAngles = new Vector3(0,0, 0);
        //    }

        //    if (calcVelocity.y < -maxFallVelocity)
        //    {
        //        calcVelocity.y = -maxFallVelocity;
        //    }
        //    rb2d.velocity = calcVelocity;
        //    if (coyoteTimeCurrent > 0 && jumpBufferTimeCurrent > 0 && !jumping)
        //    {
        //        //dustJump.gameObject.SetActive(true);
        //        //dustJump.Play();
        //        musicManager.Instance.PlayJump();

        //        jumping = true;
        //        rb2d.gravityScale = normalGravity;
        //        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        //        rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //        coyoteTimeCurrent = jumpBufferTimeCurrent = 0;
        //    }
        //}
        //else
        //{
        //    musicManager.Instance.StopWalking();
        //}

    }


    public IEnumerator rotateParticle(bool direction)
    {
        isRotating = true;
        if (direction)
        {
            for (int i = 0; i < 10; i++)
            {
                yield return (new WaitForSeconds(.5f / 10));
                //dustWalk.transform.eulerAngles += new Vector3(0, 18, 0);
            }
            //dustWalk.transform.eulerAngles = new Vector3(0, 180, 0);
            isRotating = false;
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                yield return (new WaitForSeconds(.5f / 10));
                //dustWalk.transform.eulerAngles -= new Vector3(0, 18, 0);
            }
            //dustWalk.transform.eulerAngles = new Vector3(0, 0, 0);
            isRotating = false;
        }


    }
    public void restart()
    {
        print("plyerRestart");
        transform.position = initialPos;
        gameObject.SetActive(true);
        Head.SetActive(true);
        gunOBJ.SetActive(true);
        GetComponentInChildren<IGun>().restart();
        GetComponentInChildren<PlayerShoot>().
        GetComponentInChildren<PlayerShoot>().restart();
        GetComponent<ACharacterLife>().restart();
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
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
                UpdateAnimatorBool("isRunning", false);

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
            if (float.IsNaN(vel.x))
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
        UpdateAnimatorSpeed(use);

        //maxSpeed*=mult;
        //acceleration*=mult;
        //groundDecceleration*=mult;
        //turnDecceleration*=mult;

        //airDecceleration*=mult;
        //jumpForce*=mult;
        if (data.currentMagnitude != 1)
        {
            normalGravity = mult = 1.5f;
            fallGravity = mult = 2;
            jumpForce = 5;
        }
        else
        {
            normalGravity = mult = 4;
            fallGravity = mult = 8;
            jumpForce = 8;
        }
    }

    public void UpdateAnimatorFloat(string property, float value)
    {
        //bodyAnim.SetFloat(property, value);
        headAnim.SetFloat(property, value);
        armAnim.SetFloat(property, value);
        parentAnim.SetFloat(property, value);
    }
    public void UpdateAnimatorBool(string property, bool value)
    {
        //bodyAnim.SetBool(property, value);
        headAnim.SetBool(property, value);
        armAnim.SetBool(property, value);
        parentAnim.SetBool(property, value);
    }
    public void UpdateAnimatorSpeed(float speed)
    {
        //bodyAnim.speed = speed;
        headAnim.speed = speed;
        armAnim.speed = speed;
        parentAnim.speed = speed;
    }

}

