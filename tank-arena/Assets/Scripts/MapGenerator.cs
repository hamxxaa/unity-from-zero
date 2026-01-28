using UnityEngine;
using Unity.AI.Navigation;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Settings")]
    [SerializeField] GameObject wallPrefab;
    [SerializeField] int width = 20;
    [SerializeField] int height = 20;
    [SerializeField] float blockSize = 2f;

    [Header("Randomness")]
    [SerializeField] int seed = 10;
    [Range(0, 1)]
    [SerializeField] float wallDensity = 0.15f;

    [Header("Navigation Mesh")]
    [SerializeField] NavMeshSurface surface;

    private List<GameObject> spawnedWalls = new List<GameObject>();

    public void GenerateMap()
    {
        foreach (GameObject wall in spawnedWalls)
        {
            Destroy(wall);
        }
        spawnedWalls.Clear();

        Random.InitState(seed);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 pos = new Vector3(
                    (-width * blockSize / 2f) + (x * blockSize),
                    1f,
                    (-height * blockSize / 2f) + (z * blockSize)
                );

                // Edges of the map must be walls 
                if (x == 0 || x == width - 1 || z == 0 || z == height - 1)
                {
                    SpawnWall(pos);
                }
                // Spawn area must be clear
                else if (Vector3.Distance(new Vector3(x, 0, z), new Vector3(width / 2, 0, height / 2)) < 3f)
                {
                }
                else if (Random.value < wallDensity)
                {
                    SpawnWall(pos);
                }
            }
        }

        if (surface != null)
        {
            surface.BuildNavMesh();
        }
    }

    void SpawnWall(Vector3 position)
    {
        GameObject newWall = Instantiate(wallPrefab, position, Quaternion.identity, transform);
        spawnedWalls.Add(newWall);
    }
}