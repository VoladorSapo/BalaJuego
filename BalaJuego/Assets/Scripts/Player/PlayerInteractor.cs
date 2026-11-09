
using System.Collections;
using UnityEngine;
using TMPro;
using System.Linq;
using System.ComponentModel;
using Unity.Collections;
using System;
using UnityEngine.Events;
public class PlayerInteractor : CharacterInteractor
{

    //[SerializeField] private float slowDownMagnitude;
    [SerializeField] TMP_Text textoaviso;
    [SerializeField] private PlayerInput playerInput;

    IGameState stateManager;

    [SerializeField] LayerMask clickable;

    ObjectDetector<IInteractable> grabDetector;
    botleDetector botleDetector;


    public EnemyDetector stunedDetector;


    [SerializeField] bool reloading;


    Throwable bottleToGrab;
    [SerializeField] GameObject BottlePrefab;


    ACharacterLife enemyMelee;

    [field: SerializeField] public Animator anim { get; private set; }

    [field:SerializeField]   public GameObject[] hidewhenMelee {  get; private set; }

    [SerializeField] public bool hasBottle;
    float stopBufferTimeCurrent;
    [SerializeField] float stopBufferTime;


    [SerializeField] protected ParticleSystem bulletPick;

    GameObject executionCamera;

    public float shakeIntensity;


    //If player is in a state where they can shoot (not rolling, stuned, etc)
    [SerializeField] private bool canShoot;

    protected override void Awake()
    {

        base.Awake();
        executionCamera = GetComponentInChildren<Unity.Cinemachine.CinemachineCamera>().gameObject;
        playerGunChangeEvent = new UnityEvent<characterGunChangeData>();
        enemyMelee = null;
    }
    private void Start()
    {
        textoaviso.enabled = false;
        hasBottle = false;
        executionCamera.SetActive(false);
        shoot = GetComponentInChildren<IGun>();
        stateManager = ServiceLocator.Instance.Get<IGameState>();
        grabDetector = GetComponentInChildren<ObjectDetector<IInteractable>>();
        //botleDetector = GetComponentInChildren<botleDetector>();

        stunedDetector = GetComponentInChildren<EnemyDetector>();
        ServiceLocator.Instance.Get<IGameState>().subscribeToStateChange(changeState);
        reloading = false;
        grabDetector.gameObject.SetActive(false);

        playerInput = GetComponent<PlayerInput>();

    }
    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    shootBufferTimeCurrent = shootBufferTime;
        //}
        //else
        //{
        //    shootBufferTimeCurrent -= Time.deltaTime;   
        //}

        if (playerInput.ShootDown)
        {
            print("shootpressed");
            if (!reloading && canShoot && settingManager.Instance.gameConfiguration.shootAndUseEquipmentStates.Contains(stateManager.getState()))
            {
                if (currentEquipment != null)
                {
                    currentEquipment.GetComponent<IEquipable>().Action(GetComponent<CharacterInteractor>(), GetComponentInChildren<gunRotate>().transform.eulerAngles.z);
                }
                else
                {

                    if (shoot.shoot())
                    {
                        textoaviso.enabled = false;
                    }
                }
            }
        }
        if (playerInput.InteractDown)
        {
            if (!reloading && canShoot && settingManager.Instance.gameConfiguration.interactableGameStates.Contains(stateManager.getState()))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickable);
                if (hit)
                {
                    IInteractable interactableObject = hit.collider.GetComponentInParent<IInteractable>();
                    print(hit.collider.gameObject.name + " " + interactableObject + " " + grabDetector.reachableObjects.Contains(interactableObject));

                    if (interactableObject != null && grabDetector.reachableObjects.Contains(interactableObject) && interactableObject.CanInteract())
                    {
                        print("interact");
                        interactableObject.interact(this);
                    }

                }
            }
        }
        if (playerInput.StopTimeDown)
        {
            stopBufferTimeCurrent = stopBufferTime;

        }
        else
        {
            stopBufferTimeCurrent -= Time.deltaTime;
        }
        if (stopBufferTimeCurrent > 0)
        {
            if (settingManager.Instance.gameConfiguration.stopTimeStates.Contains(stateManager.getState()) && (shoot.getBullets() == 0 || settingManager.Instance.gameConfiguration.canStopTimeWithGunFull))
            {
                stopBufferTimeCurrent = 0;
                musicManager.Instance.PlaySound("snd_startslowtime");
                ServiceLocator.Instance.Get<ITimeManager>().changeTimeMagnitude(settingManager.Instance.gameConfiguration.slowDownTimeMagnitude);
                textoaviso.enabled = false;

            }
            if (shoot.getBullets() > 0 && !settingManager.Instance.gameConfiguration.canStopTimeWithGunFull)
            {
                textoaviso.enabled = true;
            }
        }
        if (playerInput.StopTimeUp)
        {
            stopBufferTimeCurrent = 0;
            textoaviso.enabled = false;
            if (stateManager.getState() == IGameState.gameState.SlowDown && shoot.getBullets() == 0)
            {
                musicManager.Instance.PlaySound("snd_stopslowtime");
                ServiceLocator.Instance.Get<ITimeManager>().changeTimeMagnitude(1);
            }
        }
        if (playerInput.MeleeDown)
        {
            if (stunedDetector.reachableObjects.Count > 0 && GetComponent<PlayerMove>().onGround)
            {
                killMelee(stunedDetector.reachableObjects[0].getObj().GetComponent<ACharacterLife>());
            }
        }
        // print(playerInput.PauseDown);
        if (playerInput.PauseDown)
        {
            ServiceLocator.Instance.Get<IGameState>().Pause();
        }



    }
    public void killMelee(ACharacterLife enemy)
    {
        executionCamera.SetActive(true);
        musicManager.Instance.PlaySoundPitch("snd_melee");
        GetComponent<PlayerMove>().playerMoveStateMachine.SetState(new PlayerExecuteState(null));
        enemyMelee = enemy;
        enemy.GetComponent<ACharacterLife>().meleeDeath();
    }
    public void endMeleeAnim()
    {
        GetComponent<PlayerMove>().playerMoveStateMachine.ForceEndState(new PlayerExecuteState(null));
        //foreach (GameObject item in hidewhenMelee)
        //{
        //    item.SetActive(true);
        //}


    }
    public void getBullet()
    {
        musicManager.Instance.PlaySoundPitch("snd_reload");
        reloading = true;
        bulletPick.Play();
        anim.Play("gunReload", anim.GetLayerIndex("B_Arm_Actions"));
        anim.GetComponent<AnimatorLayerManager>().PickLayer("Base_gun");
        changePlayerGun(true);
        ServiceLocator.Instance.Get<ITimeManager>().changeTimeMagnitude(1);

    }
    public override void getEquipment(GameObject interactableObj)
    {
        base.getEquipment(interactableObj);
        reloading = false;
        //anim.Play("armGrabObject", anim.GetLayerIndex("B_Arm_Actions"));
        //anim.GetComponent<AnimatorLayerManager>().PickLayer("Grab_Object");
        bulletPick.Play();
    }
   
    public void endReloadAnim()
    {
        ServiceLocator.Instance.Get<IsoftLock>().checkAll();
        reloading = false;
    }
    public void throwObject()
    {
        print("throwObject");
        anim.Play("armThrowObject", anim.GetLayerIndex("B_Arm_Actions"));
        if (shoot.getBullets() == 0) anim.GetComponent<AnimatorLayerManager>().ResetPickfromLayers();
        else anim.GetComponent<AnimatorLayerManager>().PickLayer("Base_gun");
        currentEquipment.SetActive(true);
        IProyectile equipmentProyectile = currentEquipment.GetComponent<IProyectile>();
        loseEquipment();
        equipmentProyectile.ActivateProyectileMovement(GetComponent<CharacterInteractor>(), GetComponentInChildren<BaseGun>().spawnPoint.position, GetComponentInChildren<gunRotate>().transform.eulerAngles.z);
        if (anim.HasState("gunIdle")) anim.Play("gunIdle");

    }
    void changeState(object sender, stateData data)
    {
        if (settingManager.Instance.gameConfiguration.interactableGameStates.Contains(data.currentState))
        {
            grabDetector.gameObject.SetActive(true);
        }
        else if (data.currentState == IGameState.gameState.Paused)
        {
        }
        else
        {
            grabDetector.gameObject.SetActive(false);

        }
    }
    public void restart()
    {
        //textoaviso.enabled = false;
        if (textoaviso.enabled == true)
        {
            StartCoroutine(waitTurnOFfBulletAdvice());
        }
        changePlayerGun(false);
        reloading = false;
        enemyMelee = null;
        hasBottle = false;
        canShoot = true;
    }
    IEnumerator waitTurnOFfBulletAdvice()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(5f / 10f);
            if (textoaviso.enabled == false)
            {
                yield break;
            }
        }
        textoaviso.enabled = false;
    }

    public void returnToNormalCamera()
    {
        executionCamera.SetActive(false);
    }

    internal void endShootAnimation()
    {
        changePlayerGun(shoot.getBullets() > 0);
    }

    public void setCanShoot(bool _canShoot)
    {
        canShoot = _canShoot;
    }
}

public class characterGunChangeData
{
    public bool hasSomething;
    public IEquipable equipment;

    public characterGunChangeData(bool hasSomething, IEquipable equipment)
    {
        this.hasSomething = hasSomething;
        this.equipment = equipment;
    }
}
