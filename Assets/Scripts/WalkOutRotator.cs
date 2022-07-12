using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkOutRotator : MonoBehaviour
{
    private float rotationAngle;

    private float currRotation;

    public bool rotateUp;

    public bool tester;

    public bool isLocked = true;
    // Start is called before the first frame update
    void Start()
    {
        currRotation = this.transform.localRotation.eulerAngles.x;
        if (rotateUp)
        {
            rotationAngle = -6f;
        } else {
            rotationAngle = 6f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (tester){
            startRotation();
            tester = false;
        }
        // If rotation is not in a locked state rotate the piece
        if(!isLocked)
        {
            // Increment the relative rotation angle
            currRotation = Mathf.Clamp(currRotation + rotationAngle, -90f, 90f);

            Vector3 eulers = this.transform.localRotation.eulerAngles;
            

            Debug.Log(Mathf.Abs(currRotation));
            if (Mathf.Abs(currRotation)> 89.9f)
            {
                isLocked = true;
            } else if (Mathf.Abs(currRotation) < 3f)
            {
                currRotation = 0f;
                isLocked = true;
            }

            float nextX = 360f + currRotation;
            this.transform.localRotation = Quaternion.Euler(new Vector3(nextX,eulers.y,eulers.z));
        }
    }

    public void startRotation()
    {
        rotationAngle *= -1;
        isLocked = false;
    }

    public void lockRotation()
    {
        isLocked = true;
    }
}
