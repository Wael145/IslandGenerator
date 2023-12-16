using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Overlays;
using UnityEngine;
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
    public float treeDensityMultiplier = 5f;
    public float rockDensityMultiplier = 2f;
    private List<Vector3> vertices;
    private float[,] usedMap;
    private float level;
    public TerrainType[] regions;
    private List<Vector3> occupiedPositions;
    private void Start()
    {
        occupiedPositions = new List<Vector3>();
        vertices = new List<Vector3>();
        GenerateMap();
        PlaceTrees();
        PlaceRocks();
    }

    private void Update()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        float[,] noiseMap = PerlinNoise.PerlinNoiseMap(width, height, seed, perlinScale, offset, octaves, persistance, lacunarity);
        usedMap= noiseMap;  
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
    public void PlaceTrees()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                level = PerlinNoise.GetLevel(usedMap, i, j);
                float treeDensity = CalculateTreeDensity(level);
                Vector3 treePosition = vertices[j * width + i];
                if (!PerlinNoise.isWater(usedMap, i, j))
                {
                    if (level > 0.45 && level <=0.65) 
                    {
                        treeDensity *= 1.5f; 
                    }
                    else if (level > 0.65 && level < 0.8)
                    {
                        treeDensity *= 1.2f;
                    }
                    else if (level >= 0.8) 
                    {               
                        treeDensity *= 0.4f; 
                    }
                    
                    if (Random.Range(0f, 1f) < treeDensity)
                    {
                        if (!IsOccupied(treePosition))
                        {
                            GenerateTreesAtPosition(treePosition, treeDensity);
                            MarkPositionAsOccupied(treePosition);
                        }
                    }
                }
                else
                {
                    vertices[j * width + i] = Vector3.zero;
                }

            }
        }
    }
    public void PlaceRocks()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                level = PerlinNoise.GetLevel(usedMap, i, j);
                float rockDensity = CalculateRockDensity(level);
                Vector3 rockPosition = vertices[j * width + i];
                if (!PerlinNoise.isWater(usedMap, i, j))
                {
                    if (level >= 0.7 && level <= 0.8)
                    {
                        rockDensity *= 0.7f;
                    }
                    else if (level > 0.45 && level < 0.7)
                    {
                        rockDensity *= 1.2f; 
                    }
                    else if (level > 0.8) 
                    {
                        rockDensity *= 0.3f; 
                    }
                    if (Random.Range(0f, 1f) < rockDensity)
                    {
                        if (!IsOccupied(rockPosition))
                        {
                            GenerateRocksAtPosition(rockPosition, rockDensity);
                            MarkPositionAsOccupied(rockPosition);
                        }
                    }
                }
            }
        }
    }
    private bool IsOccupied(Vector3 position)
    {
        return occupiedPositions.Contains(position);
    }
    private void MarkPositionAsOccupied(Vector3 position)
    {
        occupiedPositions.Add(position);
    }
    private void GenerateTreesAtPosition(Vector3 position, float treeDensity)
    {
        int numberOfTrees = Mathf.RoundToInt(treeDensity * 5);
        List<Vector3> treePositions = new List<Vector3>();

        for (int i = 0; i < numberOfTrees; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(0f, 0.05f), 0f,0f);
            Vector3 treePosition = position + randomOffset;
            treePosition.y=position.y;
            if (!IsOccupied(treePosition))
            {
                treePositions.Add(treePosition);
                MarkPositionAsOccupied(treePosition);
            }
        }

        TreesGenerator.GenerateTrees(treePositions, numberOfTrees);
    }
    private float CalculateTreeDensity(float terrainHeight)
    {
        return Mathf.PerlinNoise(terrainHeight * treeDensityMultiplier, 0);
    }
    private void GenerateRocksAtPosition(Vector3 position,float rockdensity)
    {
        int numberOfRocks = Mathf.RoundToInt(rockdensity * 4);
        List<Vector3> rockPositions = new List<Vector3>();

        for (int i = 0; i < numberOfRocks; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(0f, 0.1f), 0.0f, 0f);

            Vector3 rockPosition = position + randomOffset;
            if (!IsOccupied(rockPosition))
            {
                MarkPositionAsOccupied(rockPosition);
                rockPositions.Add(rockPosition);
            }
        }
        RocksGenerator.GenerateRocks(rockPositions,numberOfRocks);
    }
    private float CalculateRockDensity(float terrainHeight)
    {
        return Mathf.PerlinNoise(terrainHeight * rockDensityMultiplier, 0);
    }
    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color color;
        
    }
}
