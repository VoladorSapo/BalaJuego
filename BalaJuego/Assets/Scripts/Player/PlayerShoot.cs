using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    IShoot shoot;
    IGameState stateManager;

    [SerializeField] LayerMask clickable;
    private void Start()
    {
        shoot = GetComponentInChildren<IShoot>();
        stateManager = ServiceLocator.Instance.Get<IGameState>();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (stateManager.getState() == IGameState.gameState.NormalTime)
            {
                shoot.shoot();
            }
            if (true || stateManager.getState() == IGameState.gameState.SlowDown)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, clickable);

                if (hit)
                {
                    print(hit.collider.gameObject.name);
                }
            }
        }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (stateManager.getState() == IGameState.gameState.NormalTime && shoot.getBullets() == 0)
                {
                    ServiceLocator.Instance.Get<ITimeManager>().changeTimeMagnitude(0.2f);
                }
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                if (stateManager.getState() == IGameState.gameState.SlowDown && shoot.getBullets() == 0)
                {
                    ServiceLocator.Instance.Get<ITimeManager>().changeTimeMagnitude(1);
                }
            }

        

    }
}
