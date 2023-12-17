using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR;
using static DisplayMap;

public class DisplayMap : MonoBehaviour
{
    public int height;
    public int width;
    public float[,] noiseMap;
    public int octaves;
    [Range(-2f, 2f)]
    public float persistance;
    [Range(-10f, 10f)]
    public float lacunarity;
    [Range(1f,100f)]
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;
    [Range(1, 15)]
    public int invLOD;
    private Mesh mesh;
    public int seed;
    public Vector2 offset;
    public float perlinScale;
    public float treeDensityMultiplier = 1f;
    public float rockDensityMultiplier = 1f;
    private List<Vector3> vertices;
    private float[,] treeNoiseMap;
    private float[,] usedMap;
    private float level;
    private bool canUpdate;
    public TerrainType[] regions;
    private List<Vector3> occupiedPositions;
    private float treeSize = 0.2f;
    private void Start()
    {
        occupiedPositions = new List<Vector3>();
        vertices = new List<Vector3>();
        GenerateMap();
        PlaceTrees();
        PlaceRocks();
        PlaceGrass();
        UpdatePreviousVertices();
        canUpdate = false;
    }

    private void Update()
    {
         GenerateMap();
        //if (canUpdate)
        //{
        //    DestroyAllObjects("Tree");
        //    DestroyAllObjects("Rock");
        //    DestroyAllObjects("Grass");
        //    ClearOccupiedPositions();
        //    UpdatePreviousVertices();
        //    PlaceTrees();
        //    PlaceRocks();
        //    PlaceGrass();
            
        //}
    }

    public void GenerateMap()
    {
        float[,] noiseMap = PerlinNoise.PerlinNoiseMap(width, height, seed, perlinScale, offset, octaves, persistance, lacunarity);
        treeNoiseMap = PerlinNoise.PerlinNoiseMap(width, height, seed + 1, perlinScale, offset, octaves, persistance, lacunarity);
        usedMap = noiseMap;
        Color[] colorMap = new Color[width * height];
        float[] heights = new float[width * height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float currentHeight = noiseMap[i, j];
                
                for (int terrainType = 0; terrainType < regions.Length; terrainType++)
                {
                    if (currentHeight <= regions[terrainType].height)
                    {
                        colorMap[j * width + i] = regions[terrainType].color;
                        
                        heights[j * width + i] = regions[terrainType].height;
                        break;
                    }
                }
            }
        }
        Texture2D noiseTexture = PerlinNoise.PerlinColorTexture(colorMap, width, height);
        //GetComponent<Renderer>().material.mainTexture = noiseTexture;
        GetComponent<MeshFilter>().mesh = MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, invLOD);
        vertices = GetComponent<MeshFilter>().mesh.vertices.ToList();
        mesh = GetComponent<MeshFilter>().mesh;
        
        GetComponent<MeshRenderer>().material.mainTexture = noiseTexture;
    }
    public void PlaceGrass()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 grassPosition = vertices[j * width + i];
                if (!PerlinNoise.isWater(treeNoiseMap, i, j) && IsGrass(treeNoiseMap, i, j))
                {
                    GenerateGrassAtPosition(grassPosition);
                }
                else
                {
                    vertices[j * width + i] = Vector3.zero;
                }
            }
        }
    }
    
    private void PlaceTrees()
    {
        Dictionary<float, List<Vector3>> treesByLevel = new Dictionary<float, List<Vector3>>();
        for (int i = 0; i < width-1; i++)
        {
            for (int j = 0; j < height-1; j++)
            {
                level = PerlinNoise.GetLevel(usedMap, i, j);

                if (!PerlinNoise.isWater(usedMap, i, j))
                {
                    float treeDensity = CalculateTreeDensity(level);

                    if (level > 0.45 && level <= 0.65)
                    {
                        treeDensity *= 10f;
                    }
                    else if (level > 0.65 && level < 0.8)
                    {
                        treeDensity *= 0.8f; 
                    }
                    else if (level >= 0.8)
                    {
                        treeDensity *= 0.0f; 
                    }
                    Vector3 treePosition = vertices[j * width + i];
                    if (Random.Range(0f, 1f) < treeDensity)
                    {
                        if (!IsObjectPositionOccupied(treePosition,treeSize) && !IsObjectCollision(treePosition,treeSize))
                        {
                            if (!treesByLevel.ContainsKey(level))
                            {
                                treesByLevel[level] = new List<Vector3>();
                            }
                            treesByLevel[level].Add(treePosition);
                            MarkPositionAsOccupied(treePosition, treeSize);
                        }
                    }
                }
                else
                {
                    vertices[j * width + i] = Vector3.zero;
                }
            }
        }

        foreach (var kvp in treesByLevel)
        {
            GenerateLevelTrees(kvp.Key, kvp.Value);
        }
    }
    private bool IsObjectCollision(Vector3 objectPosition, float size)
    {
        foreach (Vector3 existingObjectPosition in occupiedPositions)
        {
            if (Vector3.Distance(objectPosition, existingObjectPosition) < treeSize)
            {
                return true; 
            }
        }
        return false; 
    }
    private void GenerateLevelTrees(float level, List<Vector3> treePositions)
    {
        int numberOfTrees = Mathf.Max(treePositions.Count, 1);
        List<Vector3> selectedTreePositions = GetRandomSubset(treePositions, numberOfTrees);
        TreesGenerator.GenerateTrees(selectedTreePositions);
    }
    private List<T> GetRandomSubset<T>(List<T> list, int count)
    {
        List<T> subset = new List<T>();

        for (int i = 0; i < Mathf.Min(count, list.Count); i++)
        {
            int randomIndex = Random.Range(0, list.Count);
            subset.Add(list[randomIndex]);
            list.RemoveAt(randomIndex);
        }
        return subset;
    }

    private void MarkPositionAsOccupied(Vector3 position, float size)
    {
        float radius = size / 2.0f;
        for (float x = position.x - radius; x <= position.x + radius; x++)
        {
            for (float z = position.z - radius; z <= position.z + radius; z++)
            {
                Vector3 occupiedPosition = new Vector3(x, position.y, z);
                occupiedPositions.Add(occupiedPosition);
            }
        }
    }
    
    private bool IsObjectPositionOccupied(Vector3 position,float size)
    {
        foreach (Vector3 pos in occupiedPositions)
        {
            if (Vector3.Distance(position, pos) < size)
            {
                return true;
            }
        }
        return false; 
    }

    private void PlaceRocks()
    {
        Dictionary<float, List<Vector3>> rocksByHeight = new Dictionary<float, List<Vector3>>();

        for (int i = 0; i < width-1; i++)
        {
            for (int j = 0; j < height-1; j++)
            {
                level = PerlinNoise.GetLevel(usedMap, i, j);

                if (!PerlinNoise.isWater(usedMap, i, j))
                {
                    float rockDensity = CalculateRockDensity(level);

                    // Adjust rock density based on terrain level
                    rockDensity *= RockDensity(level);

                    Vector3 rockPosition = vertices[j * width + i];
                    if (Random.Range(0f, 1f) < rockDensity)
                    {
                        
                        if (!IsFull(rockPosition) && !IsObjectCollision(rockPosition, 0.1f))
                        {
                            if (!rocksByHeight.ContainsKey(level))
                            {
                                rocksByHeight[level] = new List<Vector3>();
                            }
                            rocksByHeight[level].Add(rockPosition);
                            MarkPositionAsOccupied(rockPosition,0.1f);
                        }
                    }
                }
                else
                {
                    vertices[j * width + i] = Vector3.zero;
                }
            }
        }

        foreach (var pos in rocksByHeight)
        {
            GenerateRocks(pos.Key, pos.Value);
        }
    }
    private void GenerateRocks(float level, List<Vector3> rockPositions)
    {
       
        RocksGenerator.GenerateRocks(rockPositions);
    }
    private float RockDensity(float terrainLevel)
    {
        if (terrainLevel >= 0.65 && terrainLevel < 0.8)
        {
            return 0.8f; 
        }
        return 0.0f; 
    }

    private bool IsGrass(float[,] map, int x, int y)
    {
        float currentHeight = PerlinNoise.GetLevel(map, x, y);
        return currentHeight < 0.7;
    }
    private bool IsFull(Vector3 position)
    {
        return occupiedPositions.Contains(position);
    }
    private void MarkPositionAsOccupied(Vector3 position)
    {
        occupiedPositions.Add(position);
    }  
    private float CalculateTreeDensity(float terrainHeight)
    {
        return Mathf.PerlinNoise(terrainHeight * treeDensityMultiplier, 0);
    }
    private void GenerateGrassAtPosition(Vector3 position)
    {
        int numberOfHerbs = 2;
        List<Vector3> grassPositions = new List<Vector3>();

        for (int i = 0; i < numberOfHerbs; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(0f, 0.8f), 0.0f, 0f);

            Vector3 grassPosition = position + randomOffset;
            if (!IsFull(grassPosition))
            {

                MarkPositionAsOccupied(grassPosition);
                grassPositions.Add(grassPosition);
            }
        }

        GrassFoliage.GenerateFoliage(grassPositions, numberOfHerbs);
    }
    private float CalculateRockDensity(float terrainHeight)
    {
        return Mathf.PerlinNoise(terrainHeight * rockDensityMultiplier, 0);
    }
    
    private void UpdatePreviousVertices()
    {
        vertices = GetComponent<MeshFilter>().mesh.vertices.ToList();
    }
    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color color;
        
    }
}
