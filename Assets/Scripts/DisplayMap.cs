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

    [Range(1f,100f)]
    public float meshHeightMultiplier;

    public AnimationCurve meshHeightCurve;
    [Range(1, 15)]
    public int invLOD;
    Mesh mesh;
    public int seed;
    public Vector2 offset;
    public float perlinScale;

    List<Vector3> vertices;
    List<Vector3> zone1;
    List<Vector3> zone2;
    List<Vector3> zone3;
    List<Vector3> zone4;
    List<Vector3> zone5;

    float level;
    public TerrainType[] regions;
    private void Start()
    {
        vertices = new List<Vector3>();
        zone1 = new List<Vector3>();
        zone2 = new List<Vector3>(); 
        zone4 = new List<Vector3>(); 
        zone5 = new List<Vector3>();
        zone3 = new List<Vector3>();
        GenerateMap();
        TreesGenerator.GenerateTrees(zone1,80);
        TreesGenerator.GenerateTrees(zone2,60);
        TreesGenerator.GenerateTrees(zone3,50);
        TreesGenerator.GenerateTrees(zone4,20);
        TreesGenerator.GenerateTrees(zone5,10);
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
        GetComponent<MeshFilter>().mesh = MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, invLOD);
        vertices = GetComponent<MeshFilter>().mesh.vertices.ToList();
        mesh = GetComponent<MeshFilter>().mesh;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                level = PerlinNoise.GetLevel(noiseMap, i, j);
                if ((PerlinNoise.isWater(noiseMap, i, j)))
                { 
                    vertices[j * width + i] = Vector3.zero;
                }
                else if (level > 0.4 && level <= 0.55)
                {
                    zone1.Add(vertices[j * width + i]);
                }
                else if(level >0.55 && level<=0.65)
                {
                    zone2.Add(vertices[j * width + i]);
                }
                else if(level>0.65 && level<=0.7)
                {
                    zone3.Add(vertices[j * width + i]);
                }
                else if (level> 0.7 && level <=0.85)
                {
                    zone4.Add(vertices[j * width + i]);
                }
                else if (level> 0.85 && level <=1)
                {
                    zone5.Add(vertices[j * width + i]);
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
