using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneSize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 v = GetComponent<Renderer>().bounds.size; 

        BoxCollider2D b = GetComponent<Collider2D>() as BoxCollider2D;

        b.size = v;   
    }
}
