using UnityEngine;
using UnityEngine.Events;

public class Parreable :MonoBehaviour
{
  [SerializeField]  EnemyLife enemy;

    private void Start()
    {
        enemy = GetComponentInParent<EnemyLife>();
    }

    public void interact(PlayerInteractor player)
    {   
           player.killMelee(enemy);
        
    }
}