using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //gerer les collisions
        //si collision avec arbre
        if (other.CompareTag("Tree"))
        {
            gameObject.transform.position += new Vector3(Random.Range(-0.1f, 0.1f), 0f, 0);
        }
        //Si collision avec plantes
        if (other.CompareTag("Grass"))
        {
            other.transform.position += new Vector3(Random.Range(-0.1f, 0.5f), 0f, 0);
        }
        //Si collision avec rochers
        if (other.CompareTag("Rock"))
        {
            Destroy(other);
        }
    }
}
