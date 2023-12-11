using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PerlinNoise
{
    public static float[,] PerlinNoiseMap(int width, int height, float perlinScale, float offsetX, float offsetY)
    {
        float[,] noiseMap = new float[width,height];

        if (perlinScale <= 0)
            perlinScale = 0.0001f;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float Xcoord = (float)(i - width / 2f) * perlinScale / width + offsetX + 100000;
                float Ycoord = (float)(j - height / 2f) * perlinScale / height + offsetY + 100000;
                float noise = Mathf.PerlinNoise(Xcoord, Ycoord);
                noiseMap[i,j] = noise;
            }
        }
        return noiseMap;
    }


    public static Texture2D PerlinGrayTexture(float[,] noiseMap)
    {
        Texture2D noiseTexture = new Texture2D(noiseMap.GetLength(0), noiseMap.GetLength(1));

        noiseTexture.filterMode = FilterMode.Point;

        for (int i = 0; i < noiseMap.GetLength(0); i++)
            for (int j = 0;j < noiseMap.GetLength(1); j++)
            {
                Color noiseColor = new Color(noiseMap[i,j], noiseMap[i, j], noiseMap[i, j]);
                noiseTexture.SetPixel(i, j, noiseColor);
            }
        noiseTexture.Apply();

        return noiseTexture;
    }

    public static Texture2D PerlinColorTexture(float[,] noiseMap)
    {
        Texture2D noiseTexture = new Texture2D(noiseMap.GetLength(0), noiseMap.GetLength(1));

        noiseTexture.filterMode = FilterMode.Point;

        for (int i = 0; i < noiseMap.GetLength(0); i++)
            for (int j = 0; j < noiseMap.GetLength(1); j++)
            {
                Color noiseColor = MeshGenerator.ChooseColorFromHeight(noiseMap[i,j]);
                noiseTexture.SetPixel(i, j, noiseColor);
            }
        noiseTexture.Apply();

        return noiseTexture;
    }
}
