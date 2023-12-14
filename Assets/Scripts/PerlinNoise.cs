using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;
using Color = UnityEngine.Color;

public class PerlinNoise
{
    public static float[,] PerlinNoiseMap(int width, int height, int seed, float perlinScale, Vector2 offset, int octaves, float persistance, float lacunarity)
    {
        float[,] noiseMap = new float[width,height];

        if (perlinScale <= 0)
        {
            perlinScale = 0.0001f;
        }

        System.Random prng = new System.Random(seed);
        Vector2[] octavesOffsets = new Vector2[octaves];
        for (int k = 0; k < octaves; k++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octavesOffsets[k] = new Vector2(offsetX, offsetY);
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int k = 0; k < octaves; k++)
                {
                    float Xcoord = (float)(i - width / 2f) * perlinScale * frequency / width + octavesOffsets[k].x + 100000;
                    float Ycoord = (float)(j - height / 2f) * perlinScale * frequency / height + octavesOffsets[k].y + 100000;
                    float noise = Mathf.PerlinNoise(Xcoord, Ycoord) * 2 - 1;

                    noiseHeight += noise * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;

                }

                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;

                if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;

                noiseMap[i, j] = noiseHeight;

            }
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                noiseMap[i,j] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[i,j]);
            }
        }
                return noiseMap;
    }
    public static Texture2D PerlinColorTexture(Color[] colorMap, int width, int height)
    {
        Texture2D noiseTexture = new Texture2D(height, width);

        noiseTexture.filterMode = FilterMode.Point;
        noiseTexture.wrapMode = TextureWrapMode.Clamp;

        noiseTexture.SetPixels(colorMap);
        noiseTexture.Apply();

        return noiseTexture;
    }
    public static bool isWater(float[,] noiseMap, int i, int j)
    { 
        if (noiseMap[i, j] < 0.4)
        {
            return true;
        }
        return false;
    }
    public static float GetLevel(float[,] noiseMap, int i, int j)
    {
        return noiseMap[i,j];
    }

    public static float[,] TreesNoiseMap(int width, int height)
    {
        float treeNoiseScale = .05f;
        float[,] noiseMap = new float[width, height];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * treeNoiseScale + xOffset, y * treeNoiseScale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }
        return noiseMap;
    }
}
