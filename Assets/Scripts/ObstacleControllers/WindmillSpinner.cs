using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillSpinner : MonoBehaviour, IObstacle
{
    public float animationSpeed
    {
        get;
        set;
    }

    public bool clockwise;

    // Start is called before the first frame update
    void Start()
    {
        animationSpeed = 1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(animationSpeed);
        if (clockwise)
        {
            transform.Rotate(animationSpeed ,0,0);
        } else {
            transform.Rotate(-animationSpeed ,0,0);
        }
    }
}
