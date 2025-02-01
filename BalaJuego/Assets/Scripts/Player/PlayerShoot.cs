using System.Collections;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
   public IShoot shoot { get; private set; }
    IGameState stateManager;

    [SerializeField] LayerMask clickable;

    ObjectDetector<IBullet> grabDetector;

    EnemyParentDetector stunedDetector;

    cursorController cursor;
    private void Awake()
    {
        cursor = FindObjectOfType<cursorController>();
    }
    private void Start()
    {
        shoot = GetComponentInChildren<IShoot>();
        stateManager = ServiceLocator.Instance.Get<IGameState>();
        grabDetector = GetComponentInChildren<ObjectDetector<IBullet>>();
        stunedDetector = GetComponentInChildren<EnemyParentDetector>();
        ServiceLocator.Instance.Get<IGameState>().subscribeToStateChange(changeState);

        grabDetector.gameObject.SetActive(false);


    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (stateManager.getState() == IGameState.gameState.NormalTime || stateManager.getState() == IGameState.gameState.Tutorial)
            {
                if (shoot.shoot())
                {
                    cursor.empty();
                }
            }
            if (stateManager.getState() == IGameState.gameState.SlowDown || stateManager.getState() == IGameState.gameState.Tutorial)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickable);

                if (hit)
                {
                    IBullet bul = hit.collider.GetComponent<IBullet>();
                    if (grabDetector.reachableObjects.Contains(bul))
                    {
                        bul.tryGrab(this);
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
            if(stunedDetector.reachableObjects.Count > 0)
            {

                stunedDetector.reachableObjects[0].GetComponent<CharacterLife>().Die();
                //Muerte Melee
            }
        }

        

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

    
}
