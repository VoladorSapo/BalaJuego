using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneMusicCaller : MonoBehaviour
{
  [SerializeField]  float _time;
    public void PlaySound(string soundName)
    {
      ServiceLocator.Instance.Get<IcutsceneManager>().PlaySound(soundName);
    }
    public void PlayPaso()
    {
        StartCoroutine(PlayMuchoSound(_time));
    }
    IEnumerator PlayMuchoSound(float time)
    {

        for (int i = 0; i < 50; i++)
        {
            musicManager.Instance.PlayWalk();
            yield return new WaitForSecondsRealtime(time);
        }
    }
}
