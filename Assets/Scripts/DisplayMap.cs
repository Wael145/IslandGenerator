using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    [Range(1, 15)]
    public int invLOD;
    Mesh mesh;
    public int seed;
    public Vector2 offset;
    public float perlinScale;
    List<Vector3> vertices;
    public TerrainType[] regions;
    private void Start()
    {
        vertices = new List<Vector3>();
        GenerateMap();
        TreesGenerator.GenerateTrees(vertices);
    }

    private void Update()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        float[,] noiseMap = PerlinNoise.PerlinNoiseMap(width, height, seed, perlinScale, offset, octaves, persistance, lacunarity);

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
        GetComponent<MeshFilter>().mesh = MeshGenerator.GenerateTerrainMesh(noiseMap, invLOD);
        vertices = GetComponent<MeshFilter>().mesh.vertices.ToList();
        mesh = GetComponent<MeshFilter>().mesh;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if ((PerlinNoise.isWater(noiseMap, i, j)))
                { 
                    vertices[j * width + i] = Vector3.zero;
                }
            }
        }
        GetComponent<MeshRenderer>().material.mainTexture = noiseTexture;
    }

    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color color;
        
    }
}
