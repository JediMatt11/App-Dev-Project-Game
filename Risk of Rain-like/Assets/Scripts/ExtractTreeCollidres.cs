using UnityEngine;
using Unity.AI.Navigation;
using System.Collections.Generic;

public class TerrainTreeNavMeshGenerator : MonoBehaviour
{
    public Terrain terrain;

    private List<GameObject> spawnedBlockers = new List<GameObject>();

    [ContextMenu("Generate Tree Blockers (Accurate)")]
    public void Generate()
    {
        Clear();

        if (terrain == null)
        {
            Debug.LogError("No terrain assigned.");
            return;
        }

        var data = terrain.terrainData;
        var trees = data.treeInstances;
        var prototypes = data.treePrototypes;

        Debug.Log("Tree count: " + trees.Length);

        for (int i = 0; i < trees.Length; i++)
        {
            var tree = trees[i];
            var proto = prototypes[tree.prototypeIndex];

            if (proto.prefab == null) continue;

            // Get prefab bounds
            var renderer = proto.prefab.GetComponentInChildren<Renderer>();
            if (renderer == null) continue;

            Bounds bounds = renderer.bounds;

            float radius = bounds.extents.x * tree.widthScale;
            float height = bounds.size.y * tree.heightScale;

            Vector3 worldPos = Vector3.Scale(tree.position, data.size) + terrain.transform.position;

            GameObject blocker = new GameObject("TreeBlocker_" + i);
            blocker.transform.position = worldPos;

            var volume = blocker.AddComponent<NavMeshModifierVolume>();

            volume.center = new Vector3(2f, height / 2f, 2f);
            volume.size = new Vector3(radius * 2.3f, height + 40f, radius * 2.3f);

            volume.area = 1;

            spawnedBlockers.Add(blocker);
        }

        Debug.Log("Generated blockers: " + spawnedBlockers.Count);
    }

    [ContextMenu("Clear Blockers")]
    public void Clear()
    {
        foreach (var obj in spawnedBlockers)
        {
            if (obj != null)
                DestroyImmediate(obj);
        }

        spawnedBlockers.Clear();
    }
}