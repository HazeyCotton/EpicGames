using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinningSquareController : MonoBehaviour, IObstacle
{

    // Prefab used for the segments of the tube
    public spinningSquare spinningSquare2Prefab;

    // number of squares in the tube
    public int numSquares;

    // value we rotate at per frame
    public float rotValue;

    // initial z degree rotation offset at the beginning of spawn
    public float initialOffset;

    // get all the square segments we make to handle them all
    private GameObject[] squares;

    // position in hsb color scale that the colors cycle through in the tube
    private int[] colorPosition;

    // bool for whether were going to change color while playing
    public bool activeColorChange;

    private bool[] rotationTracker;

    private int squareCounter;

    public float CoolOffTime = 0.05f;

    float LastShotTime = 0f;
    
    // Animation speed of the obstacle
    // Applied in this obstacle by when we speed up the rotation rate increases
    public float animationSpeed
    {
        get;
        set;
    }

    // Start is called before the first frame update
    void Start()
    {
        activeColorChange = false;
        animationSpeed = 1f;
        rotValue = 3f;
        initialOffset = 15f;
        squares = new GameObject[numSquares];
        colorPosition = new int[numSquares*4];
        for (int i = 0; i < (numSquares * 4); i++)
        {
            colorPosition[i] = i;
        }
        for (int x = 0; x < numSquares; x++)
        {
            

            var sqr = Instantiate<spinningSquare>(spinningSquare2Prefab);
            sqr.transform.parent = this.gameObject.transform;

            sqr.transform.localPosition = new Vector3(
                0f,
                0f,
                0.6f * x);

            squares[x] = sqr.gameObject;


            squares[x].transform.localEulerAngles = new Vector3(
                squares[x].transform.localEulerAngles.x,
                squares[x].transform.localEulerAngles.y,
                squares[x].transform.localEulerAngles.z);// +(x*initialOffset));

        
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if ((Time.timeSinceLevelLoad > (LastShotTime + CoolOffTime))){
            if (squareCounter < numSquares)
            {
                squares[squareCounter].gameObject.GetComponent<spinningSquare>().startRotating();

                squareCounter++;

                LastShotTime = Time.timeSinceLevelLoad;
            } else {
                squareCounter = 0;
            }
        }

        /*
        for (int x = 0; x < numSquares; x++)
        {

            spinningSquare square2 = squares[x].gameObject.GetComponent<spinningSquare>();

            square2.startRotating();

        
        }*/
        if (activeColorChange) 
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            
            for (int x = 0; x < (numSquares*4); x++)
            {
                Renderer r = renderers[x];

                r.material.color = Color.HSVToRGB(colorPosition[x]/256f, 1f, 1f); 
                //r.material.color = Color.HSVToRGB(0.5f, 0.5f,0.5f); // color blue color


                colorPosition[x] = (colorPosition[x] +1 ) % 255;
                //r.enabled = false; // like disable it for example. 
            }
        }
    }
}
