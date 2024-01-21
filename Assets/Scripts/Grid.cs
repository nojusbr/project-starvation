using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour
{
    [Header("Spawnpoints")]
    public GameObject spawnpoint;
    public GameObject player;
    public GameObject teleporterR;
    public GameObject teleporterD;

    [Header("Resources")]
    public float resourceNoiseScale = .05f;
    public float resourceDensity = .5f;

    [Header("References")]
    public GameObject[] resourcePrefabs;
    public Material terrainMaterial;
    public Material edgeMaterial;
    Cell[,] grid;

    [Header("Specifics")]
    public float waterLevel = .4f;
    public float scale = .1f;
    public int size = 100;
    public Color groundColor = Color.green;

    void Start()
    {
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }

        float[,] falloffMap = new float[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float xv = x / (float)size * 2 - 1;
                float yv = y / (float)size * 2 - 1;
                float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
                falloffMap[x, y] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
            }
        }

        grid = new Cell[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = noiseMap[x, y];
                noiseValue -= falloffMap[x, y];
                bool isWater = noiseValue < waterLevel;
                Cell cell = new Cell(isWater);
                grid[x, y] = cell;
            }
        }

        Vector3 worldGeneratorPosition = transform.position;

        DrawTerrainMesh(grid, worldGeneratorPosition);
        DrawEdgeMesh(grid, worldGeneratorPosition);
        DrawTexture(grid, worldGeneratorPosition);
        GenerateResources(grid, worldGeneratorPosition);
        SpawnPlayer(worldGeneratorPosition);
        SpawnTeleporter(worldGeneratorPosition);
    }

    void Update()
    {
        // Check if the B key is pressed
        if (Input.GetKeyDown(KeyCode.B))
        {
            // Teleport the player back to the spawn point
            TeleportPlayerToSpawnPoint();
        }
    }

    void TeleportPlayerToSpawnPoint()
    {
        int spawnX = Random.Range(0, size);
        int spawnY = Random.Range(0, size);

        Cell cell = grid[spawnX, spawnY];

        if (!cell.isWater)
        {
            // Move the player to the new spawn point
            player.transform.position = new Vector3(spawnX, 0, spawnY) + transform.position;

            // Find and delete the spawn point
            GameObject spawnPointToDelete = GameObject.Find("SpawnPoint(Clone)");
            GameObject spawnPointToDelete2 = GameObject.Find("Empty(Clone)");
            if (spawnPointToDelete != null)
            {
                Destroy(spawnPointToDelete);
            }
            else
            {
                Debug.LogWarning("Spawn point not found for deletion.");
            }
            if (spawnPointToDelete2 != null)
            {
                Destroy(spawnPointToDelete2);
            }
            else
            {
                Debug.LogWarning("Spawn point not found for deletion.");
            }
        }
        else
        {
            // Handle the case when the new spawn point is water (you may want to add more logic here)
            Debug.LogWarning("Cannot teleport player to a water cell. Adjusting teleportation...");
            SpawnPlayer(transform.position);
        }
    }

    void DrawTerrainMesh(Cell[,] grid, Vector3 worldGeneratorPosition)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater)
                {
                    Vector3 a = new Vector3(x - .5f, 0, y + .5f);
                    Vector3 b = new Vector3(x + .5f, 0, y + .5f);
                    Vector3 c = new Vector3(x - .5f, 0, y - .5f);
                    Vector3 d = new Vector3(x + .5f, 0, y - .5f);
                    Vector2 uvA = new Vector2(x / (float)size, y / (float)size);
                    Vector2 uvB = new Vector2((x + 1) / (float)size, y / (float)size);
                    Vector2 uvC = new Vector2(x / (float)size, (y + 1) / (float)size);
                    Vector2 uvD = new Vector2((x + 1) / (float)size, (y + 1) / (float)size);
                    Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                    Vector2[] uv = new Vector2[] { uvA, uvB, uvC, uvB, uvD, uvC };
                    for (int k = 0; k < 6; k++)
                    {
                        vertices.Add(v[k]);
                        triangles.Add(triangles.Count);
                        uvs.Add(uv[k]);
                    }
                }
            }
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

        mesh.RecalculateNormals();

        MeshFilter meshFilt = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRender = gameObject.AddComponent<MeshRenderer>();

        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;

        meshFilter.transform.position = worldGeneratorPosition;
        meshCollider.transform.position = worldGeneratorPosition;

    }

    void DrawEdgeMesh(Cell[,] grid, Vector3 worldGeneratorPosition)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater)
                {
                    if (x > 0)
                    {
                        Cell left = grid[x - 1, y];
                        if (left.isWater)
                        {
                            Vector3 a = new Vector3(x - .5f, 0, y + .5f);
                            Vector3 b = new Vector3(x - .5f, 0, y - .5f);
                            Vector3 c = new Vector3(x - .5f, -1, y + .5f);
                            Vector3 d = new Vector3(x - .5f, -1, y - .5f);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                    if (x < size - 1)
                    {
                        Cell right = grid[x + 1, y];
                        if (right.isWater)
                        {
                            Vector3 a = new Vector3(x + .5f, 0, y - .5f);
                            Vector3 b = new Vector3(x + .5f, 0, y + .5f);
                            Vector3 c = new Vector3(x + .5f, -1, y - .5f);
                            Vector3 d = new Vector3(x + .5f, -1, y + .5f);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                    if (y > 0)
                    {
                        Cell down = grid[x, y - 1];
                        if (down.isWater)
                        {
                            Vector3 a = new Vector3(x - .5f, 0, y - .5f);
                            Vector3 b = new Vector3(x + .5f, 0, y - .5f);
                            Vector3 c = new Vector3(x - .5f, -1, y - .5f);
                            Vector3 d = new Vector3(x + .5f, -1, y - .5f);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                    if (y < size - 1)
                    {
                        Cell up = grid[x, y + 1];
                        if (up.isWater)
                        {
                            Vector3 a = new Vector3(x + .5f, 0, y + .5f);
                            Vector3 b = new Vector3(x - .5f, 0, y + .5f);
                            Vector3 c = new Vector3(x + .5f, -1, y + .5f);
                            Vector3 d = new Vector3(x - .5f, -1, y + .5f);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }
                        }
                    }
                }
            }
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        GameObject edgeObj = new GameObject("Edge");
        edgeObj.transform.SetParent(transform);

        edgeObj.transform.position = worldGeneratorPosition;

        MeshFilter meshFilter = edgeObj.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = edgeObj.AddComponent<MeshRenderer>();
        meshRenderer.material = edgeMaterial;
    }

    void DrawTexture(Cell[,] grid, Vector3 worldGeneratorPosition)
    {
        Texture2D texture = new Texture2D(size, size);
        Color[] colorMap = new Color[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (cell.isWater)
                    colorMap[y * size + x] = Color.blue;
                else
                    colorMap[y * size + x] = groundColor;
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colorMap);
        texture.Apply();

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = terrainMaterial;
        meshRenderer.material.mainTexture = texture;
        meshRenderer.transform.position = worldGeneratorPosition;
    }

    void GenerateResources(Cell[,] grid, Vector3 worldGeneratorPosition)
    {
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * resourceNoiseScale + xOffset, y * resourceNoiseScale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater)
                {
                    float v = Random.Range(0f, resourceDensity);
                    if (noiseMap[x, y] < v)
                    {
                        GameObject prefab = resourcePrefabs[Random.Range(0, resourcePrefabs.Length)];
                        GameObject resource = Instantiate(prefab, transform);
                        resource.transform.position = new Vector3(x, 0, y);
                        resource.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                        resource.transform.localScale += Vector3.one * Random.Range(.8f, 1.2f);
                        resource.transform.position = new Vector3(x, 0, y) + worldGeneratorPosition;
                    }
                }
            }
        }
    }

    void SpawnPlayer(Vector3 worldGeneratorPosition)
    {
        int attempts = 0;
        int maxAttempts = 100;

        while (attempts < maxAttempts)
        {
            int spawnX = Random.Range(0, size);
            int spawnY = Random.Range(0, size);


            Cell cell = grid[spawnX, spawnY];

            if (!cell.isWater)
            {
                GameObject spawnedSpawnPoint = Instantiate(spawnpoint, new Vector3(spawnX, 0, spawnY), Quaternion.identity);

                player.transform.position = new Vector3(spawnedSpawnPoint.transform.position.x, spawnedSpawnPoint.transform.position.y, spawnedSpawnPoint.transform.position.z);

                spawnedSpawnPoint.transform.position += worldGeneratorPosition;
                player.transform.position = spawnedSpawnPoint.transform.position;

                return;
            }

            attempts++;


        }
    }

    void SpawnTeleporter(Vector3 worldGeneratorPosition)
    {
        int attempts = 0;
        int maxAttempts = 100;

        while (attempts < maxAttempts)
        {
            int spawnXR = Random.Range(0, size);
            int spawnXD = Random.Range(0, size);
            int spawnYR = Random.Range(0, size);
            int spawnYD = Random.Range(0, size);


            Cell cellD = grid[spawnXR, spawnYR];
            Cell cellR = grid[spawnXR, spawnYR];

            if (!cellR.isWater)
            {
                GameObject spawnedTeleporterR = Instantiate(teleporterR, new Vector3(spawnXR, 0, spawnYR), Quaternion.identity);

                spawnedTeleporterR.transform.position += worldGeneratorPosition;

                return;
            }

            if (!cellD.isWater)
            {
                GameObject spawnedTeleporterD = Instantiate(teleporterD, new Vector3(spawnXD, 0, spawnYD), Quaternion.identity);

                spawnedTeleporterD.transform.position += worldGeneratorPosition;

                return;
            }
            attempts++;
        }
    }

}
