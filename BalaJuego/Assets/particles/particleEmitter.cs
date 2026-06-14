using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleEmitter : MonoBehaviour
{
    [SerializeField] Transform stepParticleParent;
    Dictionary<PhysicsMaterial2D, GameObject> instantiatedWalkParticles, instantiatedWalkbackParticles;
    [SerializeField] private LayerMask groundLayer;
    materialDatabase materialDatabase;
    [SerializeField] Collider2D groundCast;
    [SerializeField] Transform playerTransform;
    // Start is called before the first frame update
    void Awake()
    {
        materialDatabase = GetComponent<materialDatabase>();
        instantiatedWalkParticles = new Dictionary<PhysicsMaterial2D, GameObject>();
        instantiatedWalkbackParticles = new Dictionary<PhysicsMaterial2D, GameObject>();
    }
    public PhysicsMaterial2D GetFloorMaterial()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        filter.SetLayerMask(groundLayer);
        filter.useLayerMask = true;

        Collider2D[] result = new Collider2D[1];


        int colisionesEncontradas = groundCast.OverlapCollider(filter, result);

        if (colisionesEncontradas > 0 && result[0] != null)
        {
            return result[0].sharedMaterial;
        }

        return null;
    }
    public void EmitStep()
    {
        PhysicsMaterial2D mat = GetFloorMaterial();

        if (mat == null)
        {
            Debug.Log("Error encontrando el material de Ground Cast");
            return;
        }
        if (!instantiatedWalkParticles.ContainsKey(mat))
        {
            floorMaterial floorMat = materialDatabase.GetFloorMaterial(mat);
            if (floorMat == null)
            {
                Debug.Log("No se encontró el material en la base de datos");
                return;
            }

            GameObject stepParticle = floorMat.walkParticles;
            GameObject newParticles = Instantiate(stepParticle, stepParticleParent);
            instantiatedWalkParticles[mat] = newParticles;

        }

        GameObject particlePrefab = instantiatedWalkParticles[mat];
        EmitParticles(particlePrefab);
    }

    public void EmitStepBack()
    {
        PhysicsMaterial2D mat = GetFloorMaterial();

        if (mat == null)
        {
            Debug.Log("Error encontrando el material de Ground Cast");
            return;
        }
        if (!instantiatedWalkbackParticles.ContainsKey(mat))
        {
            floorMaterial floorMat = materialDatabase.GetFloorMaterial(mat);
            if (floorMat == null)
            {
                Debug.Log("No se encontró el material en la base de datos");
                return;
            }

            GameObject stepParticle = floorMat.walkBackParticles;
            GameObject newParticles = Instantiate(stepParticle, stepParticleParent);
            instantiatedWalkbackParticles[mat] = newParticles;

        }

        GameObject particlePrefab = instantiatedWalkbackParticles[mat];
        EmitParticles(particlePrefab);
    }

    void EmitParticles(GameObject partciclesGO)
    {
        ParticleSystem[] particles = partciclesGO.GetComponentsInChildren<ParticleSystem>(true);
        foreach (var particle in particles)
        {
            int burstCount = particle.GetComponent<BurstCount>().burstCount;
            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();

            if (playerTransform.localScale.x >= 0)
            {
                emitParams.rotation3D = new Vector3(0f, 180, 0f);
            }

            particle.GetComponent<ParticleSystem>().Emit(emitParams, burstCount);
        }
    }
}
