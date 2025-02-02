using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class lightController : MonoBehaviour
{
    Light2D light;
 [SerializeField] float intensity;
 [SerializeField] public  float dark;
    // Start is called before the first frame update
    void Start()
    {
        
        ServiceLocator.Instance.Get<ITimeManager>().subscribeToTimeChange(changeTimeMagnitude);
    }
    private void Awake()
    {
        light = GetComponent<Light2D>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void changeTimeMagnitude(object sender, timeData data)
    {
        light.intensity = data.currentMagnitude == 1 ? intensity : dark;
    }
}
