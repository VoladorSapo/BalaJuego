using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicManager : MonoBehaviour
{

    [SerializeField] AudioSource source;

    [SerializeField] float pitchMult;

    float pitchObjective;

    [SerializeField] float transitionSpeed;

    // Start is called before the first frame update
    void Start()
    {
        ServiceLocator.Instance.Get<ITimeManager>().subscribeToTimeChange(changeTimeMagnitude);
        pitchObjective = source.pitch;
    }

    // Update is called once per frame
    void Update()
    {
        if(pitchObjective != source.pitch)
        {
            source.pitch = Mathf.MoveTowards(source.pitch, pitchObjective, Time.deltaTime);
        }
    }
    void changeTimeMagnitude(object sender, timeData data)
    {
        if(data.currentMagnitude == 1)
        {
           pitchObjective = 1;
        }
        else
        {
           pitchObjective = pitchMult;
        }
    }
}
