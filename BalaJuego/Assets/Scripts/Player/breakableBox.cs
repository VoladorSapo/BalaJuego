using System.Collections;
using UnityEngine;

public class breakableBox:MonoBehaviour
{
    public ACharacterLife.Team team;
    Vector3 initPos;
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        print("breakable" + collision.name);
        if (collision.tag == "Bullet")
        {
            print("tag bullet");
            IProyectile bul = collision.GetComponent<IProyectile>();
            if (bul != null && (bul.getTeam() != team || bul.hurtAll() == true))
            {
                musicManager.Instance.PlaySoundPitch("snd_rocarompe");
                print("set active false");
                GetComponent<Animator>().Play("break");
                GetComponent<Collider2D>().enabled = false;
                Destroy(bul.getObj());
            }
        }
    }
    private void Start()
    {
        initPos = transform.position;
        ServiceLocator.Instance.Get<ILevelController>().subscribeToRestart(restart);
    }
    public void restart()
    {
        gameObject.SetActive(true);
        transform.position = initPos;
        GetComponent<Animator>().Play("idle");
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        StartCoroutine(waitrestart());
    }

    IEnumerator waitrestart()
    {
        yield return new WaitForSeconds(0.1f);
        transform.position = initPos;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().gravityScale = 1;
    }
}
