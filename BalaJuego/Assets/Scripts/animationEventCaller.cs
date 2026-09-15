
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class animationEventCaller : MonoBehaviour
{
   public ParticleSystem[] particles;
   public void endChrageHeavyEvent()
    {
        GetComponentInParent<HeavyEnemyBehaviour>().finishCharging = true;
    }

    public void endReloadEvent()
    {
        GetComponentInParent<PlayerShoot>().endReloadAnim();

    }
    public void endMeleeAnim()
    {
        ServiceLocator.Instance.Get<ILevelController>().getPlayer().GetComponent<PlayerShoot>().endMeleeAnim();
        GetComponentInParent<EnemyLife>().Die();

    }
    public void throwBottle()
    {
        GetComponentInParent<PlayerShoot>().throwObject();
    }
    public void finishDeeathAnim()
    {
        //CAMBIAR
        if (GetComponentInParent<EnemyLife>())
            GetComponentInParent<ACharacterLife>().finishDeathAnim();
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
        ServiceLocator.Instance.Get<ILevelController>().getPlayer().GetComponent<PlayerShoot>().returnToNormalCamera();
    }

    public void callCameraShake()
    {
        //float shakeIntensity = GetComponentInParent<PlayerShoot>().shakeIntensity;
        //CinemachineVirtualCamera cam =(CinemachineVirtualCamera)FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
        //StartCoroutine(shakeCamera(cam, shakeIntensity));

    }

    IEnumerator shakeCamera(Unity.Cinemachine.CinemachineCamera cam, float s)
    {
        cam.GetComponent<Unity.Cinemachine.CinemachineBasicMultiChannelPerlin>().AmplitudeGain = s;
        yield return new WaitForSeconds(.07f);
        cam.GetComponent<Unity.Cinemachine.CinemachineBasicMultiChannelPerlin>().AmplitudeGain = 0;

    }
    public void gunSpawnBullet()
    {
        GetComponentInChildren<BaseGun>().spawnBullet();
    }
    public void gunEndShoot()
    {
        GetComponentInChildren<BaseGun>().endShootAnim();
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
        //float shakeIntensity = 6;
        //Unity.Cinemachine.CinemachineCamera cam = (Unity.Cinemachine.CinemachineCamera)FindAnyObjectByType<Unity.Cinemachine.CinemachineBrain>().ActiveVirtualCamera;
        //StartCoroutine(shakeCamera(cam, shakeIntensity));

    }


    public void callCameraShakeStep()
    {
        //float shakeIntensity = 1;
        //Unity.Cinemachine.CinemachineCamera cam = (Unity.Cinemachine.CinemachineCamera)FindAnyObjectByType<Unity.Cinemachine.CinemachineBrain>().ActiveVirtualCamera;
        //StartCoroutine(shakeCamera(cam, shakeIntensity));

    }

    public void playSound(string sound)
    {
        musicManager.Instance.PlaySoundPitch(sound);

    }

    public void playParticles(int n)
    {
        particles[n].Play();
      
    }

    public void heavyStep()
    {
        musicManager.Instance.PlayHeavyWalk();
    }
}