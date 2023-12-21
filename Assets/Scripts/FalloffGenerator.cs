using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FallOffGenerator
{
    // Génération du tableau 2D qui correspond aux valeurs qu'on va retrancher à la carte du bruit de Perlin pour obtenir une île
    public static float[,] GenerateFalloffMap(int size)
    {
        float[,] falloffMap = new float[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = i / (float)size * 2 - 1;
                float y = j / (float)size * 2 - 1;

                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));

                falloffMap[i, j] = evaluatePlateau(value);
            }
        }

        return falloffMap;
    }

    public static float evaluatePlateau(float value)
    {
        float a = 3;
        float b = 2.2f;

        float value_powered = Mathf.Pow(value, a);

        return value_powered / (value_powered + Mathf.Pow((b - b * value), a));
    }

    // Création d'un gradient radial pour faire un volcan par exemple
    public static float[,] GenerateRadialGradientMap(int size)
    {
        float[,] gradientRadialMap = new float[size, size];

        Vector2 centerPoint = new Vector2(size / 2, size / 2);
        float maxDistance = 5f;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = i / (float)size * 2 - 1;
                float y = j / (float)size * 2 - 1;
                Vector2 point = new Vector2(i, j);
                float value = 1f;
                if ((centerPoint - point).magnitude < maxDistance)
                    value = (centerPoint - point).magnitude / maxDistance;

                gradientRadialMap[i, j] = 1 - value;
            }
        }

        return gradientRadialMap;
    }
}
