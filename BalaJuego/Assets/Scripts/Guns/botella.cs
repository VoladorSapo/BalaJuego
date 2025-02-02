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
        hit = false;
    }

    public void resTart(LevelAreaController _area)
    {
        area = _area;
        transform.position = initialPos;

    }
    public override void setHover(bool set)
    {
        print("setHover" + set);
            int setI = set ? 1 : 0;
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            GetComponentInChildren<SpriteRenderer>().GetPropertyBlock(block, 0);
            block.SetInt("_isOutlined", setI);
            print(GetComponentInChildren<SpriteRenderer>().name);
            GetComponentInChildren<SpriteRenderer>().SetPropertyBlock(block, 0);
            inSelect = set;
            musicManager.Instance.PlaySoundPitch("bip", 0.2f);
        if (!set)
        {
            transform.localScale = new Vector3(1, 1, 1);

        }
    }
    public override void InstantiateBullet(CharacterLife shooter, float angle)
    {
        musicManager.Instance.PlaySoundPitch("snd_lanzabotella");
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
            musicManager.Instance.PlaySoundPitch("snd_pick", 0.2f);
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
                musicManager.Instance.PlaySoundPitch("snd_botellarompe");
            }
            else
            {
                //impactParticle.Play();
                musicManager.Instance.PlaySoundPitch("snd_botellarompe");
            }
            speed = 0;
            //Destroy(gameObject, 0.5f);
        
    }
  
}


