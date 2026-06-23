using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PropScatter : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Transform propContainer;
    [SerializeField] private float yOffset;

    [SerializeField] private List<PropGroup> propGroups;

    [SerializeField] private int customSeed = 1273;
    [SerializeField] private bool useRandomSeed = true;

    [ContextMenu("Generate Props")]
    public void GenerateProps()
    {
        ClearCurrentProps();

        if (tilemap == null) return;

        if (useRandomSeed) customSeed = Random.Range(0, 9999);
        Random.InitState(customSeed);

        BoundsInt bounds = tilemap.cellBounds;

        foreach (var pos in bounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos)) continue;


            EvaluateCellForSpawning(pos);
        }

    }

    void EvaluateCellForSpawning(Vector3Int pos)
    {
        Grid layoutGrid = tilemap.layoutGrid;
        if (layoutGrid == null) return;
        Vector3 tileWorldBasePos = layoutGrid.CellToWorld(pos) + new Vector3(0, yOffset, 0);
        Debug.Log($"Comprobando celda {layoutGrid.CellToWorld(pos)}");
        tileWorldBasePos += layoutGrid.CellToLocalInterpolated(tilemap.tileAnchor);

        foreach (PropGroup group in propGroups)
        {
            if (group.prefabs == null || group.prefabs.Length == 0) continue;


            //generar ruido. La escala de ruido depende del tipo de prop
            float uniqueGroupOffset = group.groupName.GetHashCode() % 1000;

            float sampleX = (tileWorldBasePos.x + customSeed + uniqueGroupOffset) * group.noiseScale;
            float sampleY = (tileWorldBasePos.y + customSeed + (uniqueGroupOffset * 0.5f)) * group.noiseScale;

            float noiseValue = Mathf.PerlinNoise(sampleX, sampleY);

            //comprobar si el valor de ruido supera el necesario para ese tipo de prop
            if (noiseValue > group.noiseThreshold)
            {
                if (Random.value <= group.spawnChance)
                {
                    SpawnPropObject(group, tileWorldBasePos);
                }
            }
        }

    }

    public void SpawnPropObject(PropGroup group, Vector3 worldPos)
    {
        GameObject chosenPrefab = group.prefabs[Random.Range(0, group.prefabs.Length)];
        float offsetX = Random.Range(-group.maxGridOffsetX, group.maxGridOffsetX);
        float offsetY = Random.Range(-group.maxGridOffsetY, group.maxGridOffsetY);
        Vector3 finalizedPosition = new Vector3(worldPos.x + offsetX, worldPos.y + offsetY, worldPos.z);

        GameObject spawnedProp = Instantiate(chosenPrefab, finalizedPosition, Quaternion.identity, propContainer);
        if (group.allowHorizontalFlip && Random.value > 0.5f)
        {
            Vector3 currentScale = spawnedProp.transform.localScale;
            spawnedProp.transform.localScale = new Vector3(-currentScale.x, currentScale.y, currentScale.z);
            SpriteRenderer sr = spawnedProp.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingLayerName = group.sortingLayer;
                if (group.overrideOrderInLayer) sr.sortingOrder = group.orderInLayer;
            }
        }

    }

    public void ClearCurrentProps()
    {
        if (propContainer == null) propContainer = this.transform;

        for (int i = propContainer.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(propContainer.GetChild(i).gameObject);
        }
    }
}
