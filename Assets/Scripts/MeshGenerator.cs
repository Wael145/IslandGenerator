using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static Vector3[] vertices;
    public static Mesh GenerateTerrainMesh(float[,] noiseMap, float meshHeightMultiplier, AnimationCurve meshHeightCurve, int invLod)
    {
        Mesh meshTerrain = new Mesh();
        int width = noiseMap.GetLength(0) / invLod;
        int height = noiseMap.GetLength(1) / invLod;
        vertices = new Vector3[width * height];
        Vector2[] uvs = new Vector2[width * height];
        Color[] colors = new Color[width * height];
        int[] triangles = new int[(width - 1) * (height - 1) * 6];

            for (int j = 0; j < height; j++)
            for (int i = 0; i < width; i++)
            {
                float jScaled = noiseMap.GetLength(1) * j / (float)(height - 1); // on veut aller jusqu'à la largeur L totale en n pas
                float iScaled = noiseMap.GetLength(0) * i / (float)(width - 1) ;
                vertices[j * width + i] = new Vector3(iScaled, meshHeightCurve.Evaluate(noiseMap[Mathf.Max((int)iScaled - 1, 0), Mathf.Max((int)jScaled - 1, 0)]) * meshHeightMultiplier, jScaled);
                uvs[j * width + i] = new Vector2(i / (float)(width - 1), j / (float)(height - 1));
                //colors[j * width + i] = ChooseColorFromHeight(noiseMap[iScaled, jScaled]);
            }

        int indexTriangle = 0;
        for (int j = 0; j < height - 1; j++)
            for (int i = 0; i < width - 1; i++)
            {
                triangles[indexTriangle] = j * width + i;
                triangles[indexTriangle + 1] = (j + 1) * width + i;
                triangles[indexTriangle + 2] = j * width + (i + 1);

                triangles[indexTriangle + 3] = j * width + (i + 1);
                triangles[indexTriangle + 4] = (j + 1) * width + i;
                triangles[indexTriangle + 5] = (j + 1) * width + (i + 1);
                indexTriangle += 6;
            }

        meshTerrain.vertices = vertices;
        meshTerrain.triangles = triangles;
        meshTerrain.uv = uvs;
        //meshTerrain.colors = colors;
        
        meshTerrain.RecalculateBounds();
        meshTerrain.RecalculateNormals();
        meshTerrain.RecalculateTangents();

        return meshTerrain;
    }
}
