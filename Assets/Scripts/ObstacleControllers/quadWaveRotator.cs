using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quadWaveRotator : MonoBehaviour, IObstacle
{
    public float animationSpeed
    {
        get;
        set;
    }

    float currRotation;
    float rotationAngle;
    public bool clockwise = false;
    // Start is called before the first frame update
    void Start()
    {
        animationSpeed = 1f;
        float temp = transform.localRotation.eulerAngles.z;
        //Debug.Log(temp);
        if (temp < 45f)
        {
            currRotation = temp;
        } else {
            currRotation = temp - 360f;
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (clockwise)
        {
            rotationAngle = (-0.6f * animationSpeed);
        } else {
            rotationAngle = (0.6f * animationSpeed);
        }

        // Increment the relative rotation angle
            currRotation = Mathf.Clamp(currRotation + rotationAngle, -30f, 30f);

            Vector3 eulers = this.transform.localRotation.eulerAngles;

            if (Mathf.Abs(currRotation)> 29.9f)
            {
                clockwise = !clockwise;
            }

            float nextZ = 360f + currRotation;
            this.transform.localRotation = Quaternion.Euler(new Vector3(eulers.x,eulers.y,nextZ));
    }
}
