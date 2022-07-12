using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPieceHandler : MonoBehaviour
{
    public WalkOutRotator[] rotationPieces;
    int numPieces;
    // Start is called before the first frame update
    int currPiece;

    public float StartTime = 0f;

    public float CoolOffTime = 0.50f;

    float LastRotationTime = 0f;

    private bool goingOut;

    void Start()
    {
        
        goingOut = false;
        numPieces = rotationPieces.Length;
        currPiece = numPieces -1;
        Debug.Log(numPieces);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Time.timeSinceLevelLoad > (LastRotationTime + CoolOffTime))
        {
            LastRotationTime = Time.timeSinceLevelLoad;

            // Rotate the current piece
            rotationPieces[currPiece].startRotation();
            if (goingOut)
            {
                if (currPiece + 1 < numPieces)
                {
                    currPiece++;
                } else {
                    goingOut = false;
                    return;
                }
            } else {
                if (currPiece - 1 > -1)
                {
                    currPiece--;
                } else {
                    goingOut = true;
                    return;
                }
            }
        }
        
    }
}
