using UnityEngine;
using UnityEngine.Events;

public class Parreable :MonoBehaviour, IInteractable
{
    public GameObject getObj() => gameObject;
  [SerializeField]  EnemyLife enemy;
    SpriteRenderer sprite;
    bool killMeleeSkipAction;
    [SerializeField] UnityEvent action;

    private void Start()
    {
        enemy = GetComponentInParent<EnemyLife>();
        sprite = GetComponentInChildren<SpriteRenderer>();    
    }
    public void setHover(bool set)
    {
        sprite.color = set ? Color.yellow : Color.white;
    }

    public void tryGrab(PlayerShoot player)
    {
        if (killMeleeSkipAction || action.GetPersistentEventCount()==0)
        {
            player.killMelee(enemy);
        }
        else {
            action.Invoke();
        }
    }
}