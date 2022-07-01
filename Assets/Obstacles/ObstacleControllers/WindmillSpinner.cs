using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillSpinner : MonoBehaviour
{
    public float rotationRate;
    public bool clockwise;

    // Start is called before the first frame update
    void Start()
    {
        rotationRate = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (clockwise)
        {
            transform.Rotate(rotationRate,0,0);
        } else {
            transform.Rotate(-rotationRate,0,0);
        }
    }
}
