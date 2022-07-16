using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinningSquare : MonoBehaviour
{
    private bool clockwise;

    private bool rotating;

    private float rotationRate;

    private float relativeRotation;

    void Start()
    {
        clockwise = true;
        rotating = false;
        rotationRate = 4f;
        relativeRotation = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rotating)
        {
            if (clockwise)
            {
                rotateSquare(-rotationRate);
            } else {
                rotateSquare(rotationRate);
            }
        }
    }

    private void rotateSquare(float rotValue)
    {
        float nextZ = transform.localEulerAngles.z + rotValue;
        relativeRotation += rotValue;
        if (clockwise) {
            if (relativeRotation <= -360f)
            {
                
                clockwise = !clockwise;
                rotating = false;
            } 
        } else {
            if (relativeRotation >= 360f)
            {
                clockwise = !clockwise;
                rotating = false;
            }
        } 
        transform.localEulerAngles = new Vector3(
                transform.localEulerAngles.x,
                transform.localEulerAngles.y,
                nextZ);
    }

    public void startRotating()
    {
        rotating = true;
    }

}
