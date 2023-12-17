using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.PlayerLoop.PreUpdate;

public class DisplayTexture : MonoBehaviour
{
    [SerializeField] public int size;
    public void GenerateMap()
    {
        float[,] noiseMap = FallOffGenerator.GenerateFalloffMap(size);

        Texture2D noiseTexture = PerlinNoise.PerlinGrayTexture(noiseMap);
        GetComponent<Renderer>().sharedMaterial.mainTexture = noiseTexture;

    }
}
