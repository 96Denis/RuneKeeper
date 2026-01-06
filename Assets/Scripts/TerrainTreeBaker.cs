using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TerrainTreeBaker : MonoBehaviour
{
    [Header("Setari")]
    public float radiusObstacol = 0.5f; // Cat de gros e copacul
    public float inaltimeObstacol = 2f;

    // Lista pentru a tine minte ce am creat, ca sa le putem sterge
    private List<GameObject> obstacoleCreate = new List<GameObject>();

    [ContextMenu("1. Genereaza Obstacole")]
    public void GenerateObstacles()
    {
        // Curatam intai daca exista deja
        ClearObstacles();

        Terrain terrain = GetComponent<Terrain>();
        if (terrain == null)
        {
            Debug.LogError("Acest script trebuie pus pe obiectul Terrain!");
            return;
        }

        TerrainData data = terrain.terrainData;

        // Trecem prin toti copacii pictati
        foreach (TreeInstance tree in data.treeInstances)
        {
            // Calculam pozitia exacta in lumea 3D
            Vector3 worldPos = Vector3.Scale(tree.position, data.size) + terrain.transform.position;
            GameObject obstacleObj = new GameObject("Tree_Obstacle");
            obstacleObj.transform.position = worldPos;
            CapsuleCollider collider = obstacleObj.AddComponent<CapsuleCollider>();
            collider.radius = radiusObstacol;
            collider.height = inaltimeObstacol;

            // Il punem pe Layer-ul Default ca sa fie vazut de Bake
            obstacleObj.layer = LayerMask.NameToLayer("Default");
            // -------------------------

            obstacleObj.transform.SetParent(this.transform);
            obstacoleCreate.Add(obstacleObj);
        }

        Debug.Log("Am generat " + obstacoleCreate.Count + " obstacole pentru copaci!");
    }

    [ContextMenu("2. Sterge Obstacole")]
    public void ClearObstacles()
    {
        // Stergem toti copiii care au nume "Tree_Obstacle"
        // Facem asta manual cautand copiii, in caz ca am pierdut lista dupa un restart
        var children = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child.name == "Tree_Obstacle") children.Add(child.gameObject);
        }

        foreach (GameObject child in children)
        {
            DestroyImmediate(child);
        }

        obstacoleCreate.Clear();
        Debug.Log("Obstacole sterse!");
    }
}