using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class animationEventCaller : MonoBehaviour
{
    [SerializeField] ParticleSystem meleeParticles, meleeParticles1;
   public void endChrageHeavyEvent()
    {
        GetComponentInParent<HeavyEnemyController>().finishCharging = true;
    }

    public void endReloadEvent()
    {
        GetComponentInParent<PlayerShoot>().endReloadAnim();

    }

    public void endMeleeAnim()
    {
        GetComponentInParent<PlayerShoot>().endMeleeAnim();

    }
    public void throwBottle()
    {
        GetComponentInParent<PlayerShoot>().throwObject();

    }
    public void finishDeeathAnim()
    {
        if(GetComponentInParent<EnemyLife>())
        GetComponentInParent<EnemyLife>().finishDeathAnim();

        if (GetComponentInParent<TutorialLife>())
            GetComponentInParent<TutorialLife>().finishDeathAnim();

    }
    public void deathHitStop(float time)
    {
        ServiceLocator.Instance.Get<IHitStop>().HitStop(time);
    }
    public void bossStun()
    {
      // GetComponentInChildren<BossShoot>().gameObject.SetActive(false);

    }

    public void returnToNormalCamera()
    {
        GetComponentInParent<PlayerShoot>().returnToNormalCamera();
    }

    public void callCameraShake()
    {
        //float shakeIntensity = GetComponentInParent<PlayerShoot>().shakeIntensity;
        //CinemachineVirtualCamera cam =(CinemachineVirtualCamera)FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
        //StartCoroutine(shakeCamera(cam, shakeIntensity));

    }

    IEnumerator shakeCamera(CinemachineVirtualCamera cam, float s)
    {
        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = s;
        yield return new WaitForSeconds(.07f);
        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;

    }

    public void callCameraShakeEnemy()
    {
        /*
        float shakeIntensity = 3;
        CinemachineVirtualCamera cam = (CinemachineVirtualCamera)FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
        StartCoroutine(shakeCamera(cam, shakeIntensity));
        */
    }
    public void callCameraShakeHit()
    {
        float shakeIntensity = 6;
        CinemachineVirtualCamera cam = (CinemachineVirtualCamera)FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
        StartCoroutine(shakeCamera(cam, shakeIntensity));

    }


    public void callCameraShakeStep()
    {
        float shakeIntensity = 1;
        CinemachineVirtualCamera cam = (CinemachineVirtualCamera)FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
        StartCoroutine(shakeCamera(cam, shakeIntensity));

    }

    public void playSound(string sound)
    {
        musicManager.Instance.PlaySoundPitch(sound);

    }

    public void playMeleeParticles(int n)
    {
        switch(n)
        {
            case 0:
                if (meleeParticles != null)
                {
                    meleeParticles.Play();
                }
                break;
            case 1:
                if (meleeParticles1 != null)
                {
                    meleeParticles1.Play();
                }
                break;
        }
      
    }

    public void heavyStep()
    {
        musicManager.Instance.PlayHeavyWalk();
    }
}
