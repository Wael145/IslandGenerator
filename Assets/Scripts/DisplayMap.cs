using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DisplayMap : MonoBehaviour
{
    public int height;
    public int width;

    public int octaves;
    [Range(-2f, 2f)]
    public float persistance;
    [Range(-10f, 10f)]
    public float lacunarity;

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
        //GetComponent<Renderer>().material.mainTexture = noiseTexture;
    }

    private void Update()
    {
        GenerateMap();
        
        //GetComponent<Renderer>().material.mainTexture = noiseTexture;
    }

    public void GenerateMap()
    {
        float[,] noiseMap = PerlinNoise.PerlinNoiseMap(width, height, seed, perlinScale, offset, octaves, persistance, lacunarity);

        Color[] colorMap = new Color[width * height];

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
                        break;
                    }
                }
            }
        }

        Texture2D noiseTexture = PerlinNoise.PerlinColorTexture(colorMap, width, height);
        //GetComponent<Renderer>().material.mainTexture = noiseTexture;
        GetComponent<MeshFilter>().mesh = MeshGenerator.GenerateTerrainMesh(noiseMap); 
        vertices = GetComponent<MeshFilter>().mesh.vertices.ToList();
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
