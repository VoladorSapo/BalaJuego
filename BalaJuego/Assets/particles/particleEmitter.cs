using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleEmitter : MonoBehaviour
{
    [SerializeField] Transform stepParticleParent;
    Dictionary<MaterialScriptableObject, GameObject> instantiatedParticles;
    [SerializeField] private LayerMask groundLayer;
    //materialDatabase materialDatabase;
    [SerializeField] Collider2D groundCast;
    [SerializeField] Transform playerTransform;
    MaterialScriptableObject lastMat;
    // Start is called before the first frame update
    void Awake()
    {
        //materialDatabase = GetComponent<materialDatabase>();
        instantiatedParticles = new Dictionary<MaterialScriptableObject, GameObject>();
    }

    void OnEnable()
    {
        lastMat = GetFloorMaterial();
        
    }
    public MaterialScriptableObject GetFloorMaterial()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        filter.SetLayerMask(groundLayer);
        filter.useLayerMask = true;

        Collider2D[] result = new Collider2D[1];


        int colisionesEncontradas = groundCast.OverlapCollider(filter, result);

        if (colisionesEncontradas > 0 && result[0] != null)
        {
            return result[0].GetComponent<MaterialInfo>().material;
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
        if (lastMat == null) return;
        if (!instantiatedParticles.ContainsKey(lastMat))
        {
            GameObject particleSet = lastMat.playerParticleSet;
            GameObject newParticles = Instantiate(particleSet, stepParticleParent);
            instantiatedParticles[lastMat] = newParticles;
        }

        GameObject particlePrefab = instantiatedParticles[lastMat];
        ParticleSet particles = particlePrefab.GetComponent<ParticleSet>();
        EmitParticles(particles.jumpParticles);
    }
    public void EmitLand()
    {
        if (lastMat == null) return;
        if (!instantiatedParticles.ContainsKey(lastMat))
        {
            GameObject particleSet = lastMat.playerParticleSet;
            GameObject newParticles = Instantiate(particleSet, stepParticleParent);
            instantiatedParticles[lastMat] = newParticles;
        }

        GameObject particlePrefab = instantiatedParticles[lastMat];
        ParticleSet particles = particlePrefab.GetComponent<ParticleSet>();
        EmitParticles(particles.landParticles);
    }


    GameObject CheckInstantiateParticleSet()
    {
        MaterialScriptableObject mat = GetFloorMaterial();
        lastMat = mat;
        if (mat == null)
        {
            Debug.Log("Error encontrando el material de Ground Cast");
            return null;
        }
        if (!instantiatedParticles.ContainsKey(mat))
        {
            GameObject particleSet = mat.playerParticleSet;
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
