using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for controlling simple moving obstacles like spinners
// and other obstacles that dont need an expensive animator

public class SimpleObstacleController : MonoBehaviour
{

    private float rotationRate;
    public bool clockwise;

    public enum ObstacleType {
        GroundSpinner,
        Windmill
    }

    public ObstacleType obType;

    // Start is called before the first frame update
    void Start()
    {
        rotationRate = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch(obType)
        {
            case ObstacleType.GroundSpinner:
                if (clockwise)
                {
                    transform.Rotate(0,rotationRate,0);
                } else {
                    transform.Rotate(0,-rotationRate,0);
                }

                break;

            case ObstacleType.Windmill:
                if (clockwise)
                {
                    transform.Rotate(rotationRate,0,0);
                } else {
                    transform.Rotate(-rotationRate,0,0);
                }
                break;
        }
    }

    public void setRotationRate(float newRotationRate)
    {
        rotationRate = newRotationRate;
    }
}