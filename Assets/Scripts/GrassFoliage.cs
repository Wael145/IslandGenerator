using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class GrassFoliage : MonoBehaviour
{
    public GameObject grassPrefab;
    public static GameObject grassPrefab_;
    public static int numberOfGrassInstances = 100;
    void Awake()
    {
        grassPrefab_ = grassPrefab;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public static void GenerateFoliage(List<Vector3> vertices,int nb)
    {
        for (int i = 0; i < nb; i++)
        {
            Vector3 vertice = vertices[Random.Range(0, vertices.Count)];

            if (vertice != Vector3.zero)
            {
                GameObject grass = Instantiate(grassPrefab_);
                grass.transform.position = new Vector3(vertice.x, vertice.y, vertice.z);
                grass.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                grass.transform.localScale = Vector3.one * Random.Range(.2f, .4f);
            }
        }
        
    }
}
