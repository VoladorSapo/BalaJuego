using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleEmitter : MonoBehaviour
{
    [SerializeField] Transform stepParticleParent;
    Dictionary<PhysicsMaterial2D, GameObject> instantiatedParticles;
    [SerializeField] private LayerMask groundLayer;
    materialDatabase materialDatabase;
    [SerializeField] Collider2D groundCast;
    [SerializeField] Transform playerTransform;
    PhysicsMaterial2D lastMat;
    // Start is called before the first frame update
    void Awake()
    {
        materialDatabase = GetComponent<materialDatabase>();
        instantiatedParticles = new Dictionary<PhysicsMaterial2D, GameObject>();
    }

    private void Start()
    {
        lastMat = GetFloorMaterial(); 
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
        GameObject particles = CheckInstantiateParticleSet();
        if (particles != null)
        {
            ParticleSet particleSet = particles.GetComponent<ParticleSet>();
            EmitParticles(particleSet.walkParticles);
        }
    }

    public void EmitStepBack()
    {
        GameObject particles = CheckInstantiateParticleSet();
        if (particles != null)
        {
            ParticleSet particleSet = particles.GetComponent<ParticleSet>();
            EmitParticles(particleSet.walkbackParticles);
        }
    }
    public void EmitJump()
    {
        if (!instantiatedParticles.ContainsKey(lastMat))
        {
            floorMaterial floorMat = materialDatabase.GetFloorMaterial(lastMat);
            if (floorMat == null)
            {
                Debug.Log("Error encontrando el material para las particulas de salto");
                return;
            }

            GameObject particleSet = floorMat.particleSet;
            GameObject newParticles = Instantiate(particleSet, stepParticleParent);
            instantiatedParticles[lastMat] = newParticles;

        }

        GameObject particlePrefab = instantiatedParticles[lastMat];
        ParticleSet particles = particlePrefab.GetComponent<ParticleSet>();
        EmitParticles(particles.jumpParticles);
    }

    GameObject CheckInstantiateParticleSet()
    {
        PhysicsMaterial2D mat = GetFloorMaterial();
        lastMat = mat;
        if (mat == null)
        {
            Debug.Log("Error encontrando el material de Ground Cast");
            return null;
        }
        if (!instantiatedParticles.ContainsKey(mat))
        {
            floorMaterial floorMat = materialDatabase.GetFloorMaterial(mat);
            if (floorMat == null)
            {
                Debug.Log("No se encontró el material en la base de datos");
                return null;
            }

            GameObject particleSet = floorMat.particleSet;
            GameObject newParticles = Instantiate(particleSet, stepParticleParent);
            instantiatedParticles[mat] = newParticles;

        }

        GameObject particlePrefab = instantiatedParticles[mat];
        return particlePrefab;
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
