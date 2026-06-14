using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "floorMaterial", menuName = "ScriptableObject/floorMaterial")]
public class floorMaterial : ScriptableObject
{
    public PhysicsMaterial2D material;
    public GameObject walkParticles;
    public GameObject walkBackParticles;
}
