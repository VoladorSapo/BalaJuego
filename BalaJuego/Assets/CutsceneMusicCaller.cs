using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneMusicCaller : MonoBehaviour
{
    public void PlaySound(string soundName)
    {
      ServiceLocator.Instance.Get<IcutsceneManager>().PlaySound(soundName);
    }
}
