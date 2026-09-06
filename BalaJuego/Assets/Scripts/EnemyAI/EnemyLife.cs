using UnityEngine;

public class EnemyLife : ACharacterLife
{
    [SerializeField] GameObject gun;
    [SerializeField] GameObject head;
    [SerializeField] GameObject countText;
    [SerializeField] ParticleSystem hitParticles;
    [SerializeField] Vector3 bodyMovePos;

    AEnemyBehaviour behaviour;
    public override void Die()
    {
        dead = true;
        behaviour.stateMachine.ForceSetState(new EnemyDeadState(behaviour));
        GetComponent<AEnemyBehaviour>().stunedCollider.enabled = false;
        // collider.gameObject.SetActive(false);
        if (melee)
        {

            spriteParent.SetActive(true);
            if (GetComponent<GunEnemyBehaviour>() != null)
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
            if (GetComponent<GunEnemyBehaviour>() != null)
            {
                countText.SetActive(false);
                //gun.SetActive(false);
                head?.SetActive(false);
            }
            if (GetComponent<HeavyEnemyBehaviour>() != null)
            {
                head?.SetActive(false);
            }
            anim.Play("enemyDie");
            hitParticles.Play();
            //Animacion morir
        }
    }
    public override void finishDeathAnim()
    {
        if (melee)
        {
            PlayerMove player = ServiceLocator.Instance.Get<ILevelController>().getPlayer().GetComponent<PlayerMove>();
            transform.position = player.transform.position + new Vector3(bodyMovePos.x * -player.transform.localScale.x, bodyMovePos.y, bodyMovePos.z);
            transform.localScale = new Vector3(player.runningDirection, 1, 1);
            GetComponent<Rigidbody2D>().isKinematic = true;
        }
        if (GetComponent<AEnemyBehaviour>().area != null)
        {
            GetComponent<AEnemyBehaviour>().area.enemyDie(GetComponent<AEnemyBehaviour>());
        }
        GetComponent<AEnemyBehaviour>().setColor(false);
        GetComponent<AEnemyBehaviour>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        // gameObject.SetActive(false);
    }
    public override void restart()
    {
        base.restart();

        if (GetComponent<GunEnemyBehaviour>() != null)
        {
            print("countext" + gameObject.name);
            countText.SetActive(true);
            gun.SetActive(true);
            head?.SetActive(true);
        }
        if (GetComponent<HeavyEnemyBehaviour>() != null)
        {
            head?.SetActive(true);
        }
        GetComponent<AEnemyBehaviour>().enabled = true;
        GetComponent<AEnemyBehaviour>().setColor(false);
        GetComponent<AEnemyBehaviour>().stunedCollider.enabled = true;


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
        GetComponent<AEnemyBehaviour>().setColor(false);
        behaviour.stateMachine.ForceSetState(new EnemyDeadState(behaviour));
        dead = true;
        anim.Play("melee");
    }
    public override void getStuned()
    {
        behaviour.startStunState();
    }
    public override void endStun()
    {
        behaviour.endStunState();

    }
    protected override void Start()
    {
        base.Start();
        behaviour = GetComponent<AEnemyBehaviour>();
    }
}
