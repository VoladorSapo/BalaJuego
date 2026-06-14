using Cinemachine;
using System.Collections;
using UnityEngine;
using TMPro;
using System.Linq;
using System.ComponentModel;
using Unity.Collections;
public class PlayerShoot : MonoBehaviour
{
  [SerializeField]  TMP_Text textoaviso;
   public IGun shoot { get; private set; }
    [SerializeField] private PlayerInput playerInput;

    IGameState stateManager;

    [SerializeField] LayerMask clickable;

    ObjectDetector<baseBullet> grabDetector;
  botleDetector botleDetector;


  public  EnemyParentDetector stunedDetector;

    cursorController cursor;

    [SerializeField] bool reloading;


    botella bottleToGrab;
    [SerializeField] GameObject BottlePrefab;


    CharacterLife enemyMelee;

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


    private void Awake()
    {
        cursor = FindObjectOfType<cursorController>();
        executionCamera = GetComponentInChildren<CinemachineVirtualCamera>().gameObject;
        enemyMelee = null;
    }
    private void Start()
    {
        textoaviso.enabled = false;
        hasBottle = false;
        executionCamera.SetActive(false);
        shoot = GetComponentInChildren<IGun>();
        stateManager = ServiceLocator.Instance.Get<IGameState>();
        grabDetector = GetComponentInChildren<ObjectDetector<baseBullet>>();
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
            if (!reloading && stateManager.getState() == IGameState.gameState.NormalTime || stateManager.getState() == IGameState.gameState.Tutorial)
            {
                if (currentEquipment != null)
                {
                    currentEquipment.GetComponent<IEquipable>().Action(GetComponent<CharacterLife>(), GetComponentInChildren<gunRotate>().transform.eulerAngles.z);
                }
                else
                {

                    if (shoot.shoot())
                    {
                        textoaviso.enabled = false;
                        cursor.empty();
                    }
                }
            }
        }
        if (playerInput.InteractDown)
        {
            if (!reloading && stateManager.getState() == IGameState.gameState.NormalTime || stateManager.getState() == IGameState.gameState.SlowDown || stateManager.getState() == IGameState.gameState.Tutorial)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickable);
                if (hit)
                {
                    IInteractable interactableObject = hit.collider.GetComponentInParent<IInteractable>();
                    print((interactableObject != null) + "" +  grabDetector.reachableObjects.Contains(interactableObject));
                    if (interactableObject != null && grabDetector.reachableObjects.Contains(interactableObject) && !(stateManager.getState() == IGameState.gameState.NormalTime))
                    {
                        interactableObject.tryGrab(this);
                    }
                    //bottleToGrab = hit.collider.GetComponentInParent<botella>();
                    //if (bottleToGrab != null && botleDetector.reachableObjects.Contains(bottleToGrab))
                    //{
                    //    bottleToGrab.tryGrab(this);
                    //}
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
                ServiceLocator.Instance.Get<ITimeManager>().changeTimeMagnitude(0.2f);
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
                if (stunedDetector.reachableObjects[0].GetComponent<HeavyEnemyController>() != null)
                {
                    executionCamera.SetActive(true);
                    anim.Play("heavyMelee");
                    musicManager.Instance.PlaySoundPitch("snd_melee");
                }
                if (stunedDetector.reachableObjects[0].GetComponent<GunEnemyController>() != null)
                {
                    executionCamera.SetActive(true);
                    anim.Play("basicEnemyMelee");
                    musicManager.Instance.PlaySoundPitch("snd_melee");
                }
                if (stunedDetector.reachableObjects[0].GetComponent<BossEnemyController>() != null)
                {
                    musicManager.Instance.PlaySoundPitch("snd_melee");
                    musicManager.Instance.FadeOutCurrentSong();
                    ServiceLocator.Instance.Get<ILevelController>().playLastCutscene();

                }
                foreach (GameObject item in hidewhenMelee)
                {
                    item.SetActive(false);
                }
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<PlayerMove>().isMeleeing = true;

                enemyMelee = stunedDetector.reachableObjects[0].GetComponent<CharacterLife>();
                stunedDetector.reachableObjects[0].GetComponent<CharacterLife>().meleeDeath();
                //Muerte Melee
            }
        }
        if (playerInput.PauseDown)
        {
            ServiceLocator.Instance.Get<IGameState>().Pause();
        }

        

    }
    public void getBullet()
    {
        musicManager.Instance.PlaySoundPitch("snd_reload");
        reloading = true;
        bulletPick.Play();
        GetComponentInChildren<IGun>().getAnim().Play("gunReload");
        cursor.full();
    }
    public void getInteractableObject(GameObject interactableObj)
    {
        reloading = true;
        GetComponentInChildren<IGun>().getAnim().Play("bottlePick");
        bulletPick.Play();
        cursor.full();
        currentEquipment = interactableObj;
        currentEquipment.transform.parent = equipmentParent;
        currentEquipment.transform.localPosition = Vector3.zero;
    }
    public void endMeleeAnim()
    {
        foreach (GameObject item in hidewhenMelee)
        {
            item.SetActive(true);
        }
        GetComponent<PlayerMove>().isMeleeing = false;
        enemyMelee.Die();
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
        currentEquipment.GetComponent<IProyectile>().InstantiateBullet(GetComponent<CharacterLife>(), GetComponentInChildren<gunRotate>().transform.eulerAngles.z, GetComponentInChildren<BaseGun>().spawnPoint.position);
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
        cursor.empty();
        reloading = false;
        enemyMelee = null;
        hasBottle = false;
        
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
}
