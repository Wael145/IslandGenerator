using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class GrassFoliage : MonoBehaviour
{
    public static GameObject[] grassPrefabs_;
    public GameObject[] grassPrefabs;
    void Awake()
    {
        grassPrefabs_ = grassPrefabs;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public static void GenerateFoliage(List<Vector3> vertices)
    {
         Vector3 vertice = vertices[Random.Range(0, vertices.Count)];

         if (vertice != Vector3.zero)
         {
             GameObject prefab = grassPrefabs_[Random.Range(0, grassPrefabs_.Length)];
             GameObject grass = Instantiate(prefab);
             grass.transform.position = new Vector3(vertice.x, vertice.y, vertice.z);
             grass.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
             grass.transform.localScale = Vector3.one * Random.Range(.2f, .8f);
         }
        
        
    }
}
