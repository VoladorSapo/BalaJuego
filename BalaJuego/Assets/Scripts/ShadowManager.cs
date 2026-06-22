using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowManager : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float maxRayDistance = 20f;
    public List<Transform> EnemyTransforms = new List<Transform>(20);


    private const int MAX_ENEMIES = 20; 
    private Vector4[] positionCache = new Vector4[MAX_ENEMIES];

    Vector4 dummyPosition = new Vector4(99999f, 99999f, 0, 999999f);
    void Update()
    {
        int count = Mathf.Min(EnemyTransforms.Count, MAX_ENEMIES);

        for (int i = 0; i < count; i++)
        {
            if (EnemyTransforms[i] != null)
            {
                RaycastHit2D hit = Physics2D.Raycast(EnemyTransforms[i].position, Vector2.down, maxRayDistance, groundLayer);

                if (hit.collider != null)
                {
                    //Shader.SetGlobalVector("_PlayerWorldPos", hit.point);
                    positionCache[i] = new Vector4(hit.point.x, hit.point.y, 0, hit.distance);
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

}
