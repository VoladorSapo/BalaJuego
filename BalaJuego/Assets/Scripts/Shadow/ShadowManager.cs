using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyShadowInfo
{
    public Transform transform;
    public float shadowSize;

    public EnemyShadowInfo(Transform transform, float shadowSize)
    {
        this.transform = transform;
        this.shadowSize = shadowSize;
    }
}
public class ShadowManager : MonoBehaviour, IShadowManger
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float maxRayDistance = 20f;
    public List<EnemyShadowInfo> EnemyTransforms = new List<EnemyShadowInfo>(20);


    private const int MAX_ENEMIES = 20; 
    private Vector4[] positionCache = new Vector4[MAX_ENEMIES];

    Vector4 dummyPosition = new Vector4(99999f, 99999f, 0, 999999f);

    public void addEnemyTransform(EnemyShadowInfo info)
    {
        EnemyTransforms.Add(info);
    }
    public void removeEnemyTransform(EnemyShadowInfo info)
    {
        EnemyTransforms.Remove(info);
    }
    void Update()
    {
        int count = Mathf.Min(EnemyTransforms.Count, MAX_ENEMIES);

        for (int i = 0; i < count; i++)
        {
            if (EnemyTransforms[i].transform != null)
            {
                RaycastHit2D hit = Physics2D.Raycast(EnemyTransforms[i].transform.position, Vector2.down, maxRayDistance, groundLayer);

                if (hit.collider != null)
                {
                    //Shader.SetGlobalVector("_PlayerWorldPos", hit.point);
                    positionCache[i] = new Vector4(hit.point.x, hit.point.y, EnemyTransforms[i].shadowSize, hit.distance);
                }
                else
                {
                    positionCache[i] = transform.position;
                }
                
            }else
            {
                positionCache[i] = dummyPosition;   
            }
        }

        Shader.SetGlobalVectorArray("_EnemyPositions", positionCache);
        Shader.SetGlobalInt("_EnemyCount", count);

        
    }

    public void Instantiate()
    {

    }
}
public interface IShadowManger : IService
{
    public void removeEnemyTransform(EnemyShadowInfo info);
    public void addEnemyTransform(EnemyShadowInfo info);
}
