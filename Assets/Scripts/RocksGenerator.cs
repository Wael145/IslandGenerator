using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocksGenerator : MonoBehaviour
{
    public static GameObject[] _spawnPoints;
    public static GameObject rock;
    public GameObject rockPrefab;

    // fill these out in your spawner manager thingy:

    void Awake()
    {
        rock = rockPrefab;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public static void GenerateRocks(List<Vector3> vertices, int nbRocks)
    {
        //float v = Random.Range(0f, treeDensity);


        for (int y = 0; y < nbRocks - 1; y++)
        {
            
            foreach(Vector3 vertice in vertices) { 
            if (vertice != Vector3.zero)
            {
                GameObject rock_ = Instantiate(rock);
                rock_.transform.position = new Vector3(vertice.x, vertice.y, vertice.z);
                rock_.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                rock_.transform.localScale = Vector3.one * Random.Range(0.05f, 0.1f);

            }}
        }

    }
}
