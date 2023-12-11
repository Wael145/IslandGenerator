using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TreesGenerator : MonoBehaviour
{
    
    public static GameObject[] TreePrefabs_;
    public GameObject[] TreePrefabs;
    public static int nbTrees =80;
    void Awake()
    {
        TreePrefabs_ = TreePrefabs;
    }
    // Start is called before the first frame update
    void Start()
    {
 
    }

    public static void GenerateTrees(List<Vector3> vertices)
    {
        for (int y = 0; y < nbTrees; y++)

        {      
               Vector3 vertice = vertices[Random.Range(0,vertices.Count)];
               GameObject prefab = TreePrefabs_[Random.Range(0, TreePrefabs_.Length)];
               GameObject tree = Instantiate(prefab);
               tree.transform.position = new Vector3(vertice.x, vertice.y, vertice.z);
               tree.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
               tree.transform.localScale = Vector3.one * Random.Range(.2f,.4f);
               
        }
    }
    // Update is called once per frame
    void Update()
    {

        
    }
}
