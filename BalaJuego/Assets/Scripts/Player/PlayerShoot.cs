using System.Collections;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
   public IShoot shoot { get; private set; }
    IGameState stateManager;

    [SerializeField] LayerMask clickable;

    ObjectDetector<baseBullet> grabDetector;
  botleDetector botleDetector;


  public  EnemyParentDetector stunedDetector;

    cursorController cursor;

    bool reloading;

    baseBullet bulletToGrab;

    botella bottleToGrab;
    [SerializeField] GameObject BottlePrefab;


    CharacterLife enemyMelee;

    [SerializeField] Animator anim;

[SerializeField]    GameObject[] hidewhenMelee;

[SerializeField]  public  bool hasBottle;
    private void Awake()
    {
        cursor = FindObjectOfType<cursorController>();
        enemyMelee = null;
    }
    private void Start()
    {
        hasBottle = false;
        shoot = GetComponentInChildren<IShoot>();
        stateManager = ServiceLocator.Instance.Get<IGameState>();
        grabDetector = GetComponentInChildren<ObjectDetector<baseBullet>>();
        botleDetector = GetComponentInChildren<botleDetector>();

        stunedDetector = GetComponentInChildren<EnemyParentDetector>();
        ServiceLocator.Instance.Get<IGameState>().subscribeToStateChange(changeState);
        reloading = false;
        grabDetector.gameObject.SetActive(false);


    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!reloading && stateManager.getState() == IGameState.gameState.NormalTime || stateManager.getState() == IGameState.gameState.Tutorial)
            {
                if (hasBottle)
                {
                    
                    hasBottle = false;
                    GetComponentInChildren<IShoot>().getAnim().Play("bottleThrow");
                }
                else
                {
                    if (shoot.shoot())
                    {
                        cursor.empty();
                    }
                }
            }
            if (!reloading && stateManager.getState() == IGameState.gameState.NormalTime || stateManager.getState() == IGameState.gameState.SlowDown || stateManager.getState() == IGameState.gameState.Tutorial)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickable);

                if (hit)
                {
                    bulletToGrab = hit.collider.GetComponentInParent<baseBullet>();
                    if (bulletToGrab != null && grabDetector.reachableObjects.Contains(bulletToGrab) && !(stateManager.getState() == IGameState.gameState.NormalTime))
                    {
                        reloading = true;
                        GetComponentInChildren<IShoot>().getAnim().Play("gunReload");
                        bulletToGrab.tryGrab(this);
                        cursor.full();

                    }
                    bottleToGrab = hit.collider.GetComponentInParent<botella>();
                    if (bottleToGrab !=null && botleDetector.reachableObjects.Contains(bottleToGrab))
                    {
                        reloading = true;
                        GetComponentInChildren<IShoot>().getAnim().Play("bottlePick");
                        hasBottle = true;
                        bottleToGrab.tryGrab(this);
                        cursor.full();

                    }
                }
            }
        }
            if (Input.GetKeyDown(KeyCode.E))
            {
            if (stateManager.getState() == IGameState.gameState.NormalTime && shoot.getBullets() == 0)
            {
                musicManager.Instance.PlaySound("snd_startslowtime");
                ServiceLocator.Instance.Get<ITimeManager>().changeTimeMagnitude(0.2f);
                }
            }
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (stateManager.getState() == IGameState.gameState.SlowDown && shoot.getBullets() == 0)
            {
                musicManager.Instance.PlaySound("snd_stopslowtime");
                ServiceLocator.Instance.Get<ITimeManager>().changeTimeMagnitude(1);
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(stunedDetector.reachableObjects.Count > 0 && GetComponent<PlayerMove>().onGround)
            {
                if (stunedDetector.reachableObjects[0].GetComponent<HeavyEnemyController>() != null)
                {

                }
                if (stunedDetector.reachableObjects[0].GetComponent<GunEnemyController>() != null)
                {
                    anim.Play("basicEnemyMelee");

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
        reloading = false;
       
    }
    public void throwBottle()
    {
        baseBullet botel = Instantiate(BottlePrefab, GetComponentInChildren<CharacterShoot>().spawnPoint.position, Quaternion.identity).GetComponent<baseBullet>();
        botel.InstantiateBullet(GetComponent<CharacterLife>(), GetComponentInChildren<gunRotate>().transform.eulerAngles.z);
        GetComponentInChildren<IShoot>().getAnim().Play("gunIdle");

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
        reloading = false;
        enemyMelee = null;
        hasBottle = false;
        
    }
    
}
