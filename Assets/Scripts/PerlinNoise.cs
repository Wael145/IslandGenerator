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

    // Cr�ation de la carte de bruit de Perlin
    public static float[,] PerlinNoiseMap(int width, int height, int seed, float perlinScale, Vector2 offset, int octaves, float persistance, float lacunarity)
    {
        float[,] noiseMap = new float[width,height];

        if (perlinScale <= 0)
        {
            perlinScale = 0.0001f;
        }

        System.Random prng = new System.Random(seed);
        Vector2[] octavesOffsets = new Vector2[octaves];

        // Initialisation des offsets des octaves pour pas que toutes les valeurs soient centr�es en (0, 0)
        for (int k = 0; k < octaves; k++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octavesOffsets[k] = new Vector2(offsetX, offsetY);
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        // Calcul des valeurs du bruit de Perlin
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                // Loop sur le nombre d'octaves
                for (int k = 0; k < octaves; k++)
                {
                    // Rescaling des coordonn�es et ajout d'un offset ainsi que d'octaves
                    float Xcoord = (float)(i - width / 2f) * perlinScale * frequency / width + octavesOffsets[k].x + 100000;
                    float Ycoord = (float)(j - height / 2f) * perlinScale * frequency / height + octavesOffsets[k].y + 100000;

                    float noise = Mathf.PerlinNoise(Xcoord, Ycoord) * 2 - 1;

                    // On ajoute du bruit suppl�mentaire par dessus le bruit initial pour avoir des montagnes et des plages avec une forme plus naturelle

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

        // Les valeurs pr�sentes dans le tableau noiseMap sont seuill�es pour �tre remises entre 0 et 1
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                noiseMap[i,j] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[i,j]);
            }
        }
        return noiseMap;
    }

    // G�n�ration d'une texture2D en nuances de gris � partir du bruit de Perlin
    public static Texture2D PerlinGrayTexture(float[,] noiseMap)
    {
        Texture2D noiseTexture = new Texture2D(noiseMap.GetLength(0), noiseMap.GetLength(1));
        noiseTexture.filterMode = FilterMode.Point;
        noiseTexture.wrapMode = TextureWrapMode.Clamp;
        for (int i = 0; i < noiseMap.GetLength(0); i++)
            for (int j = 0; j < noiseMap.GetLength(1); j++)
            {
                Color noiseColor = new Color(noiseMap[i, j], noiseMap[i, j], noiseMap[i, j]);
                noiseTexture.SetPixel(i, j, noiseColor);
            }
        noiseTexture.Apply();
        return noiseTexture;
    }

    // G�n�ration d'une texture2D en couleurs
    public static Texture2D PerlinColorTexture(Color[] colorMap, int width, int height)
    {
        Texture2D noiseTexture = new Texture2D(height, width);

        noiseTexture.filterMode = FilterMode.Point;
        noiseTexture.wrapMode = TextureWrapMode.Clamp;

        noiseTexture.SetPixels(colorMap);
        noiseTexture.Apply();

        return noiseTexture;
    }
    //verifier si c'est de l'eau
    public static bool isWater(float[,] noiseMap, int i, int j)
    { 
        if (noiseMap[i, j] < 0.45)
        {
            return true;
        }
        return false;
    }
    //Get level
    public static float GetLevel(float[,] noiseMap, int i, int j)
    {
        return noiseMap[i,j];
    }
}
