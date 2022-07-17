using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelAligner : MonoBehaviour
{
    private Vector3 rotation;
 
    void Start()
    {
        rotation = transform.eulerAngles;
        rotation.y = 0f;
    }
    
    void LateUpdate()
    {
        transform.eulerAngles = rotation;
    }
}
