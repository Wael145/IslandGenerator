using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FallOffGenerator
{
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
}
