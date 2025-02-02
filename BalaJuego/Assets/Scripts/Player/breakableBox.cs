using UnityEngine;

public class breakableBox:MonoBehaviour
{
    public CharacterLife.Team team;
    Vector3 initPos;
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        print("breakable" + collision.name);
        if (collision.tag == "Bullet")
        {
            print("tag bullet");
            IBullet bul = collision.GetComponent<IBullet>();
            if (bul != null && (bul.getTeam() != team || bul.hurtAll() == true))
            {
                print("set active false");
                GetComponent<Animator>().Play("break");
                GetComponent<Collider2D>().enabled = false;
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
        GetComponent<Collider2D>().enabled = true;

    }
}
