using UnityEngine;

public class EnemyStunedState: BaseEnemyState
{
    public EnemyStunedState(EnemyBehaviour _enemy)
    {
        enemy = _enemy;
    }
    public override void Update()
    {

    }
    public override void OnEnter()
    {
        musicManager.Instance.StopHeavyWalking();
        enemy.stunedCollider.gameObject.SetActive(true);
        enemy.anim.Play("enemyStun");
        if(enemy.GetComponent<HeavyEnemyController>() != null)
        {
            enemy.GetComponentInChildren<WallDetector>().gameObject.SetActive(false);
        }

    }
    public override void OnExit()
    {
        Debug.Log("end stun");
        enemy.stunedCollider.gameObject.SetActive(false);
      
    }
}