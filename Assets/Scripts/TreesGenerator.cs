using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static DisplayMap;

public class TreesGenerator : MonoBehaviour
{
    public static GameObject[] TreePrefabs_;
    public GameObject[] TreePrefabs;
    
    // fill these out in your spawner manager thingy:

    void Awake()
    {
        TreePrefabs_ = TreePrefabs;
    }

    //fonction pour spawn les arbres
    public static void GenerateTrees(List<Vector3> vertices)
    { 
       foreach(var vertice in vertices) { 

            if(vertice != Vector3.zero) 
            {
                GameObject prefab = TreePrefabs_[Random.Range(0, TreePrefabs_.Length)];
                GameObject tree = Instantiate(prefab);
                tree.transform.position = new Vector3(vertice.x, vertice.y, vertice.z);
                tree.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                tree.transform.localScale = Vector3.one * Random.Range(.1f, .3f);
                    
            
            }
        }

    }    
}
