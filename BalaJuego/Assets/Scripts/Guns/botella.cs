using UnityEngine;

public class botella : baseBullet
{
    public bool isThrown;
    LevelAreaController area;
    Vector3 initialPos;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        initialPos = transform.position;
        ITimeManager time = ServiceLocator.Instance.Get<ITimeManager>();
        timeMagnitude = time.getMagnitude();
        setHover(false);
        hit = false;
    }
    public override void setHover(bool set)
    {
        base.setHover(set);
    }
    public void resTart(LevelAreaController _area)
    {
        area = _area;
        transform.position = initialPos;

    }
    public override void InstantiateBullet(CharacterLife shooter, float angle)
    {
        musicManager.Instance.PlayDisparo();
        print(shooter.transform.localScale.x);
        angle *= shooter.transform.localScale.x;
        transform.eulerAngles = new Vector3(0, shooter.transform.localScale.x < 0 ? -180 : 0, angle);
        team = shooter.team;
        anim = GetComponentInChildren<Animator>(true);
        anim.Play("bottleFly");

    }

    public override void tryGrab(PlayerShoot player)
    {
        if (area)
        {
            ServiceLocator.Instance.Get<ITimeManager>().changeTimeMagnitude(1);
            area.destroyBottle(this);
            gameObject.SetActive(false);

        }
    }
    public override void hitSomething(GameObject obj)
    {
        anim.Play("bottleCrash");

        //Animacion o algo
        if (obj.GetComponent<CharacterLife>() != null)
            {
               // hitParticle.Play();
                musicManager.Instance.PlaySoundPitch("snd_contacto_enemigo");
            }
            else
            {
                //impactParticle.Play();
                musicManager.Instance.PlaySoundPitch("snd_contacto_obstaculo");
            }
            speed = 0;
            //Destroy(gameObject, 0.5f);
        
    }
  
}


