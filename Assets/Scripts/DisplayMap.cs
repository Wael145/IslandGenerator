using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayMap : MonoBehaviour
{
    public int height;
    public int width;

    public Vector2 offset;
    public float perlinScale;
    private void Start()
    {
        float[,] noiseMap = PerlinNoise.PerlinNoiseMap(width, height, perlinScale, offset.x, offset.y);
        Texture2D noiseTexture = PerlinNoise.PerlinColorTexture(noiseMap);

        GetComponent<Renderer>().material.mainTexture = noiseTexture;

        GetComponent<MeshFilter>().mesh = MeshGenerator.GenerateTerrainMesh(noiseMap);
        GetComponent<MeshRenderer>().material.mainTexture = noiseTexture;
    }

    private void Update()
    {
        float[,] noiseMap = PerlinNoise.PerlinNoiseMap(width, height, perlinScale, offset.x, offset.y);
        Texture2D noiseTexture = PerlinNoise.PerlinColorTexture(noiseMap);
        GetComponent<MeshFilter>().mesh = MeshGenerator.GenerateTerrainMesh(noiseMap);
        GetComponent<MeshRenderer>().material.mainTexture = noiseTexture;

        GetComponent<Renderer>().material.mainTexture = noiseTexture;
    }

}
