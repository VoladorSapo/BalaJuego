using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MaterialScriptableObject", menuName = "ScriptableObject/MaterialScriptableObject")]
public class MaterialScriptableObject : ScriptableObject
{
    //public PhysicsMaterial2D material;
    public GameObject playerParticleSet;
    public GameObject bulletImpactParticles;
}
