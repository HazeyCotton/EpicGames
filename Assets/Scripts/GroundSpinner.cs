using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpinner : MonoBehaviour, IObstacle
{

    private float rotationRate;
    public bool clockwise;

    public float animationSpeed
    {
        get;
        set;
    }

    // Start is called before the first frame update
    void Start()
    {
        animationSpeed = 1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (clockwise)
        {
            transform.Rotate(0,animationSpeed,0);
        } else {
            transform.Rotate(0,-animationSpeed,0);
        }
    }

    void setRotationRate(float newRotationRate)
    {
        rotationRate = newRotationRate;
    }
}
