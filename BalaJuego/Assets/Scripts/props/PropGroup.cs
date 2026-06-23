using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PropGroup
{
    //Variaciones de un mismo prop (Setos, piedras, cactus, etc.)
    public string groupName;
    public GameObject[] prefabs; 

    [Range(0f, 1f)] public float spawnChance = 0.15f; 
    public float noiseScale = 0.1f;                  
    [Range(0f, 1f)] public float noiseThreshold = 0.55f; 

    public float maxGridOffsetX = 0.35f;
    public float maxGridOffsetY = 0.35f;
    public bool allowHorizontalFlip = true;

    public string sortingLayer;
    public int orderInLayer;
    public bool overrideOrderInLayer;
}
