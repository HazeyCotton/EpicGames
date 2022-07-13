using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPieceHandler : MonoBehaviour, IObstacle
{

    public float animationSpeed
    {
        get;
        set;
    }

    public WalkOutRotator[] rotationPieces;
    int numPieces;
    // Start is called before the first frame update
    int currPiece;

    public float StartTime = 0f;

    public float CoolOffTime = 0.50f;

    float LastRotationTime = 0f;

    private bool goingOut;

    private bool waitCycle;

    void Start()
    { 
        animationSpeed = 1f;
        goingOut = false;
        waitCycle = false;
        numPieces = rotationPieces.Length;
        currPiece = numPieces -1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float adjustedCooloff = CoolOffTime - (animationSpeed -1f);
        //Debug.Log(animationSpeed);
        if(Time.timeSinceLevelLoad > (LastRotationTime + adjustedCooloff))
        {
            LastRotationTime = Time.timeSinceLevelLoad;
            if (waitCycle)
            {
                waitCycle = false;
                return;
            }
            // Rotate the current piece
            rotationPieces[currPiece].startRotation();
            if (goingOut)
            {
                if (currPiece + 1 < numPieces)
                {
                    currPiece++;
                } else {
                    goingOut = false;
                    waitCycle = true;
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
