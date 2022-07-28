using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class levelTilter : MonoBehaviour
{

    public GameObject player;
    private float xTilt;
    private float zTilt;

    private float detiltVal = 0.5f;
    private float tiltVal = 2f;
    private float maxTilt = 8f;

    private bool xMoved;
    private bool zMoved;

    // Start is called before the first frame update
    void Start()
    {
        xTilt = 0;
        zTilt = 0;

        xMoved = false;
        zMoved = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // First we see if any of the keys we care about are pressed down
        // Note: this press method allows for constant hold inputs
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {

            // Next we mark that the we have moved the angles since in almost all cases both will have been moved
            // Mark them seperate since each axis gets rotated and need to be told to stop decreasing seperately
            xMoved = true;
            zMoved = true;

            // Make a vector 2 that indicates the static value the input key represents
            Vector2 rotationVec = new Vector2(0f, 0f);

            // Set x value of vector to the necassary value
            if (Input.GetKey(KeyCode.W)) 
            { 
                rotationVec[0] = 1;
            } else if (Input.GetKey(KeyCode.S)) 
            {
                rotationVec[0] = -1;
            }
            
            // Set z value of vector to the necassary value
            if (Input.GetKey(KeyCode.A)) 
            {
                rotationVec[1] = 1;
            } else if (Input.GetKey(KeyCode.D)) 
            {
                rotationVec[1] = -1;
            }

            /*
            Adjust the xTilt and zTilt values based on the input keys transformed to the proper tilt direction
            derived from the y rotation angle of the fox

            This then gives the effect that the stage always tilts in the correct directions relative to the 
            player when view the fox, ie 
            w -> forward
            s -> backward
            a -> left
            d -> right
            */ 
            xTilt = Mathf.Clamp(xTilt 
            + (
                (Mathf.Sin(player.transform.eulerAngles.y*Mathf.PI/180f)*rotationVec[1]
                + Mathf.Cos(player.transform.eulerAngles.y*Mathf.PI/180f)*rotationVec[0]) * tiltVal), -maxTilt, maxTilt);

            zTilt = Mathf.Clamp(zTilt 
            + (
                (Mathf.Cos(player.transform.eulerAngles.y*Mathf.PI/180f)*rotationVec[1]
                - Mathf.Sin(player.transform.eulerAngles.y*Mathf.PI/180f)*rotationVec[0]) * tiltVal), -maxTilt, maxTilt);
        }


        // Decrease the x direction if needed
        if (xMoved) {
            if (xTilt > 0) {
                xTilt -= detiltVal;
            } else {
                xTilt += detiltVal;
            }
            if (Mathf.Abs(xTilt) < 1f)
            {
                xMoved = false;
                xTilt = 0f;
            }
        }
        // decrease the z direction if needed
        if (zMoved) {
            if (zTilt > 0) {    
                zTilt -= detiltVal;
            } else {
                zTilt += detiltVal;
            }

            if (Mathf.Abs(zTilt) < 1f)
            {
                zMoved = false;
                zTilt = 0f;
            }
        }
        // set the new stage rotation position
        this.transform.localEulerAngles = new Vector3(
            360f+xTilt,
            0f,
            360f+zTilt
        );

    }
}
