using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class materialDatabase : MonoBehaviour
{
    public List<floorMaterial> materials;
    
    public floorMaterial GetFloorMaterial(PhysicsMaterial2D physicMaterial)
    {
        foreach(floorMaterial mat in materials)
        {
            if (mat.material == physicMaterial)
            {
                return mat;
            }
        }
        return null;
    }
}
