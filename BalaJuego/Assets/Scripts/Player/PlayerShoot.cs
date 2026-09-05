using Cinemachine;
using System.Collections;
using UnityEngine;
using TMPro;
using System.Linq;
using System.ComponentModel;
using Unity.Collections;
using System;
using UnityEngine.Events;
public class PlayerShoot : MonoBehaviour
{

    [SerializeField] private float slowDownMagnitude;
  [SerializeField]  TMP_Text textoaviso;
   public IGun shoot { get; private set; }
    [SerializeField] private PlayerInput playerInput;

    IGameState stateManager;

    [SerializeField] LayerMask clickable;

    ObjectDetector<ABaseProyectile> grabDetector;
  botleDetector botleDetector;


  public  EnemyParentDetector stunedDetector;


    [SerializeField] bool reloading;


    Throwable bottleToGrab;
    [SerializeField] GameObject BottlePrefab;


    ACharacterLife enemyMelee;

    [SerializeField] Animator anim;

[SerializeField]    GameObject[] hidewhenMelee;

[SerializeField]  public  bool hasBottle;
    float stopBufferTimeCurrent;
    [SerializeField] float stopBufferTime;


    [SerializeField] protected ParticleSystem bulletPick;

    GameObject executionCamera;

    public float shakeIntensity;

   [SerializeField]private GameObject currentEquipment;
    [SerializeField] private Transform equipmentParent;

    protected UnityEvent<characterGunChangeData> playerGunChangeEvent;

    //If player is in a state where they can shoot (not rolling, stuned, etc)
    private bool canShoot;

    private void Awake()
    {
        executionCamera = GetComponentInChildren<CinemachineVirtualCamera>().gameObject;
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
        grabDetector = GetComponentInChildren<ObjectDetector<ABaseProyectile>>();
        botleDetector = GetComponentInChildren<botleDetector>();

        stunedDetector = GetComponentInChildren<EnemyParentDetector>();
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
            if (!reloading && canShoot && stateManager.getState() == IGameState.gameState.NormalTime || stateManager.getState() == IGameState.gameState.Tutorial)
            {
                if (currentEquipment != null)
                {
                    currentEquipment.GetComponent<IEquipable>().Action(GetComponent<ACharacterLife>(), GetComponentInChildren<gunRotate>().transform.eulerAngles.z);
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
            if (!reloading &&canShoot && stateManager.getState() == IGameState.gameState.NormalTime || stateManager.getState() == IGameState.gameState.SlowDown || stateManager.getState() == IGameState.gameState.Tutorial)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickable);
                if (hit)
                {
                    IInteractable interactableObject = hit.collider.GetComponentInParent<IInteractable>();
                    if (interactableObject != null && grabDetector.reachableObjects.Contains(interactableObject) && !(stateManager.getState() == IGameState.gameState.NormalTime))
                    {
                        interactableObject.tryGrab(this);
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
            if (stateManager.getState() == IGameState.gameState.NormalTime && shoot.getBullets() == 0)
            {
                stopBufferTimeCurrent = 0;
                musicManager.Instance.PlaySound("snd_startslowtime");
                ServiceLocator.Instance.Get<ITimeManager>().changeTimeMagnitude(slowDownMagnitude);
                textoaviso.enabled = false;

            }
            if (shoot.getBullets() > 0)
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
            if(stunedDetector.reachableObjects.Count > 0 && GetComponent<PlayerMove>().onGround)
            {
                killMelee(stunedDetector.reachableObjects[0].GetComponent<ACharacterLife>());
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


        foreach (GameObject item in hidewhenMelee)
        {
            item.SetActive(false);
        }
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<PlayerMove>().isMeleeing = true;

        enemyMelee = enemy;
        enemy.GetComponent<ACharacterLife>().meleeDeath();
    }
    public void getBullet()
    {
        musicManager.Instance.PlaySoundPitch("snd_reload");
        reloading = true;
        bulletPick.Play();
        GetComponentInChildren<IGun>().getAnim().Play("gunReload");
        changePlayerGun(true);
    }
    public void getInteractableObject(GameObject interactableObj)
    {
        print(interactableObj);
        reloading = false;
        GetComponentInChildren<IGun>().getAnim().Play("bottlePick");
        bulletPick.Play();
        IEquipable equipable = interactableObj.GetComponent<IEquipable>();
        changePlayerGun(true, equipable);
        currentEquipment = interactableObj;
        equipable.setAsEquipment(equipmentParent);
    }
    public void endMeleeAnim()
    {
        foreach (GameObject item in hidewhenMelee)
        {
            item.SetActive(true);
        }
        GetComponentInChildren<IGun>().getAnim().Play("gunReload");
        GetComponent<PlayerMove>().isMeleeing = false;
    }
    public void endReloadAnim()
    {
        ServiceLocator.Instance.Get<IsoftLock>().checkAll();
        reloading = false;
    }
    public void throwObject()
    {
        print("throwObject");
        currentEquipment.SetActive(true);
        currentEquipment.transform.parent = null;
        currentEquipment.GetComponent<IProyectile>().ActivateProyectileMovement(GetComponent<ACharacterLife>(), GetComponentInChildren<gunRotate>().transform.eulerAngles.z, GetComponentInChildren<BaseGun>().spawnPoint.position);
        currentEquipment = null;
        GetComponentInChildren<IGun>().getAnim().Play("gunIdle");
    }
    void changeState(object sender, stateData data)
    {
        if (data.currentState == IGameState.gameState.SlowDown)
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
    public void changePlayerGun(bool _hasAnything,IEquipable _equipment = null)
    {
        print("changeplayergun"+_hasAnything);
        playerGunChangeEvent.Invoke(new characterGunChangeData(_hasAnything,_equipment));
    }
    public void subscribeToPlayerGunChange(UnityAction<characterGunChangeData> response)
    {
        playerGunChangeEvent?.AddListener(response);
        
    }
    public void unSubscribeToPlayerGunChange(UnityAction<characterGunChangeData> response)
    {
        playerGunChangeEvent?.RemoveListener(response);

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