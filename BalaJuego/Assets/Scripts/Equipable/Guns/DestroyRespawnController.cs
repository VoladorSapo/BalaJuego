using System;
using System.Collections;
using UnityEngine;

public class DestroyRespawnController : MonoBehaviour
{
  [SerializeField]  bool respawn;
    Action respawnAction;
    ITimeManager timer;
    Vector3 startPosition;
    Transform startParent;
    Vector3 startRotation;
    SpriteRenderer spriteRenderer;

    string defaultsortinglayer;
    int defaultsortingorder;
    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.eulerAngles;
        respawnAction = () => { };
        startParent = transform.parent;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);
        if(spriteRenderer != null)
        {
            defaultsortinglayer = spriteRenderer.sortingLayerName;
            defaultsortingorder = spriteRenderer.sortingOrder;
        }
        timer = ServiceLocator.Instance.Get<ITimeManager>();
        ServiceLocator.Instance.Get<ILevelController>().subscribeToRestart(InvokeRespawn);
        if (respawn)
        {
            respawnAction += ()=>gameObject.SetActive(true);
        }
        else
        {
            respawnAction += () => Destroy(this.gameObject);
        }

    }
    public void InvokeRespawn()
    {
        transform.parent = startParent;
        transform.position = startPosition;
        transform.eulerAngles = startRotation;
        if(spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = defaultsortinglayer;
            spriteRenderer.sortingOrder = defaultsortingorder;
        }
        respawnAction.Invoke();
    }
    public void Destroy(float time = 0)
    {
      StartCoroutine(delayDestroy(time));
    }
    IEnumerator delayDestroy(float time = 0)
    {
        yield return new WaitForSeconds(time/ timer.getMagnitude());
        if (respawn)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void OnDestroy()
    {
        ServiceLocator.Instance.Get<ILevelController>().unSubscribeToRestart(InvokeRespawn);
    }
    public void subscribeToRespawnAction(Action action)
    {
        if(respawn)
        respawnAction+= action;
    }
    public void unsubscribeToRespawnAction(Action action)
    {
        if(respawn)
        respawnAction -= action;
    }
}