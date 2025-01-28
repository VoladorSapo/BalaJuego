using UnityEngine;

public class baseBullet: MonoBehaviour, IBullet
{
    Vector3 direction;
    [SerializeField] float speed;
    [SerializeField] int damage = 1;
    [SerializeField] bool grabable = true;
    [SerializeField] float lifeTime;
    [SerializeField] bool canHurtAll;

    CharacterLife.Team team;
    

    float timeMagnitude;

    LayerMask obstacleLayer;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((obstacleLayer & (1 << collision.gameObject.layer)) != 0)
        {
            hitSomething();
        }
    }
    private void Update()
    {
        lifeTime -= Time.deltaTime * timeMagnitude;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector2.left * speed * Time.deltaTime*timeMagnitude);
    }
    public void InstantiateBullet(CharacterLife shooter,float angle)
    {
        print(shooter.transform.localScale.x);
        angle *= shooter.transform.localScale.x;
        transform.eulerAngles = new Vector3(0, shooter.transform.localScale.x < 0 ? -180 : 0, angle);
        team = shooter.team;
    }

    public int getDamage() => damage;

    void changeTimeMagnitude(object sender, timeData data)
    {
        timeMagnitude = data.currentMagnitude;
    }
    private void Start()
    {
        ITimeManager time = ServiceLocator.Instance.Get<ITimeManager>();
        timeMagnitude = time.getMagnitude();
        time.subscribeToTimeChange(changeTimeMagnitude);

    }

    public void hitSomething()
    {
        //Animacion o algo
        Destroy(gameObject);
    }

    public CharacterLife.Team getTeam() => team;

    public bool hurtAll() => canHurtAll;

    public void tryGrab()
    {
        throw new System.NotImplementedException();
    }
}