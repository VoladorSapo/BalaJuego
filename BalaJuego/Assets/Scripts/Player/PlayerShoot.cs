using System.Collections;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
   public IShoot shoot { get; private set; }
    IGameState stateManager;

    [SerializeField] LayerMask clickable;

    ObjectDetector<IBullet> grabDetector;

    EnemyDetector stunedDetector;


    private void Start()
    {
        shoot = GetComponentInChildren<IShoot>();
        stateManager = ServiceLocator.Instance.Get<IGameState>();
        grabDetector = GetComponentInChildren<ObjectDetector<IBullet>>();
        stunedDetector = GetComponentInChildren<EnemyDetector>();
        ServiceLocator.Instance.Get<IGameState>().subscribeToStateChange(changeState);

        grabDetector.gameObject.SetActive(false);


    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (stateManager.getState() == IGameState.gameState.NormalTime)
            {
                shoot.shoot();
            }
            if (stateManager.getState() == IGameState.gameState.SlowDown)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickable);

                if (hit)
                {
                    IBullet bul = hit.collider.GetComponent<IBullet>();
                    if (grabDetector.reachableObjects.Contains(bul))
                    {
                        bul.tryGrab(this);
                    }
                }
            }
        }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (stateManager.getState() == IGameState.gameState.NormalTime && shoot.getBullets() == 0)
                {
                    ServiceLocator.Instance.Get<ITimeManager>().changeTimeMagnitude(0.1f);
                }
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                if (stateManager.getState() == IGameState.gameState.SlowDown && shoot.getBullets() == 0)
                {
                    ServiceLocator.Instance.Get<ITimeManager>().changeTimeMagnitude(1);
                }
            }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(stunedDetector.reachableObjects.Count > 0)
            {
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
