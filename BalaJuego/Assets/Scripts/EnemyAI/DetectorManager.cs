using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DetectorManager: MonoBehaviour
{
   [field:SerializeField] public Dictionary<string, ObjectDetectorBase> detectorDictionary {  get; private set; }

    internal void restart()
    {
        foreach (ObjectDetectorBase detector in detectorDictionary.Values) {

            detector.restart();
        }
    }

    //[SerializeField] List<DetectorWrapper> list;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [System.Serializable]
    class DetectorWrapper
    {
        string tag;
        ObjectDetectorBase detector;
    }
}
