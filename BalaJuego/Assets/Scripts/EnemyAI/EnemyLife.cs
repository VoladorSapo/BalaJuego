using UnityEngine;

public class EnemyLife : ACharacterLife
{
    [SerializeField] GameObject gun;
    [SerializeField] GameObject head;
    [SerializeField] GameObject countText;
    [SerializeField] ParticleSystem hitParticles;
    [SerializeField] Vector3 bodyMovePos;

    EnemyBehaviour behaviour;
    public override void Die()
    {
        dead = true;
        GetComponent<EnemyBehaviour>().stunedCollider.enabled = false;
        // collider.gameObject.SetActive(false);
        if (melee)
        {

            spriteParent.SetActive(true);
            if (GetComponent<GunEnemyController>() != null)
            {
                countText.SetActive(false);
                //gun.SetActive(false);
                head?.SetActive(false);
            }
            //Sprite muerto melee
            anim.Play("enemyDeadMelee");
            musicManager.Instance.StopHeavyWalking();

            finishDeathAnim();
        }
        else
        {
            if (GetComponent<GunEnemyController>() != null)
            {
                countText.SetActive(false);
                //gun.SetActive(false);
                head?.SetActive(false);
            }
            if (GetComponent<HeavyEnemyController>() != null)
            {
                head?.SetActive(false);
            }
            anim.Play("enemyDie");
            hitParticles.Play();
            //Animacion morir
        }
    }
    public void finishDeathAnim()
    {
        if (melee)
        {
            PlayerMove player = ServiceLocator.Instance.Get<ILevelController>().getPlayer().GetComponent<PlayerMove>();
            transform.position = player.transform.position + new Vector3(bodyMovePos.x * -player.transform.localScale.x, bodyMovePos.y, bodyMovePos.z);
            transform.localScale = new Vector3(player.runningDirection, 1, 1);
            GetComponent<Rigidbody2D>().isKinematic = true;
        }
        if (GetComponent<EnemyBehaviour>().area != null)
        {
            GetComponent<EnemyBehaviour>().area.enemyDie(GetComponent<EnemyBehaviour>());
        }
        GetComponent<EnemyBehaviour>().setColor(false);
        GetComponent<EnemyBehaviour>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        // gameObject.SetActive(false);
    }
    public override void restart()
    {
        base.restart();

        if (GetComponent<GunEnemyController>() != null)
        {
            print("countext" + gameObject.name);
            countText.SetActive(true);
            gun.SetActive(true);
            head?.SetActive(true);
        }
        if (GetComponent<HeavyEnemyController>() != null)
        {
            head?.SetActive(true);
        }
        GetComponent<EnemyBehaviour>().enabled = true;
        GetComponent<EnemyBehaviour>().setColor(false);
        GetComponent<EnemyBehaviour>().stunedCollider.enabled = true;


    }
    //int checkPlayerDir()
    //{
    //    if(FindObjectOfType<PlayerMove>().transform.position.x > gameObject.transform.position.x)
    //    {
    //        return 1;
    //    }
    //    return -1;
    //}
    public override void meleeDeath()
    {
        base.meleeDeath();
        GetComponent<EnemyBehaviour>().setColor(false);

        anim.Play("melee");
    }
    public override void getStuned()
    {
        behaviour.stateMachine.ForceSetState(new EnemyStunedState(null));
    }
    public override void endStun()
    {
        behaviour.stateMachine.ForceEndState(new EnemyStunedState(null));

    }
    protected override void Start()
    {
        base.Start();
        behaviour = GetComponent<EnemyBehaviour>();
    }
}
