using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.PlayerLoop.PreUpdate;

public class DisplayTexture : MonoBehaviour
{
    [SerializeField] public int size;
    public int width, height, seed, octaves;
    public float perlinScale;
    public float persistance;
    public float lacunarity;
    public Vector2 offset;
    public TerrainType[] regions;
    public DrawMode drawMode;
    public void GenerateMap()
    {
        float[,] noiseMap = FallOffGenerator.GenerateRadialGradientMap(size);
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

        Texture2D noiseTexture = null;
        if (drawMode == DrawMode.coloredTexture)
            noiseTexture = PerlinNoise.PerlinColorTexture(colorMap, width, height);
        else if (drawMode == DrawMode.grayTexture)
            noiseTexture = PerlinNoise.PerlinGrayTexture(noiseMap);

        GetComponent<Renderer>().sharedMaterial.mainTexture = noiseTexture;

    }

    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color color;

    }

    public enum DrawMode
    {
        coloredTexture,
        grayTexture
    }
}
