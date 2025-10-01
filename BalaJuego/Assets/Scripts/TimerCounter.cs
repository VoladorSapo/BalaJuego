using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TimerCounter : MonoBehaviour
{
    ITimer timer;
  [SerializeField]  TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        timer = ServiceLocator.Instance.Get<ITimer>();
    }

    // Update is called once per frame
    void Update()
    {
        TimePoints time = timer.getSeconds();
     text.text =   string.Format("{0:00}:{1:00}:{2:00}", time.hours, time.minutes, time.seconds);
    }
}
