using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test123 : MonoBehaviour
{
    public Transform target;
    public void Awake()
    {
        transform.parent = null;
    }
    void FixedUpdate()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
        
    }
}
