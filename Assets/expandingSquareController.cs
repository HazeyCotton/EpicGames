using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expandingSquareController : MonoBehaviour, IObstacle
{
    public float animationSpeed
    {
        get;
        set;
    }

    public expSqrPrefabScript expandingSquarePrefab;

    private GameObject[,] squares;


    public int xDim;
    public int yDim;

    private float xStep;
    private float yStep;

    private bool _expanding;

    private int counter;

    public float CoolOffTime = 1;

    float LastShotTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        animationSpeed = 1f;

        xStep = expandingSquarePrefab.transform.localScale.x;
        yStep = expandingSquarePrefab.transform.localScale.z;

        squares = new GameObject[xDim,yDim];

        _expanding = true;

        counter = 0;

        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                var sqr = Instantiate<expSqrPrefabScript>(expandingSquarePrefab);
                sqr.transform.parent = this.gameObject.transform;

                sqr.transform.localPosition = new Vector3(
                    ((( (-xDim/2f) + 0.5f) + x) * xStep),
                    0f,
                    ((( (-yDim/2f) + 0.5f) +  y) * yStep)
                );
                squares[x,y] = sqr.gameObject;
            }
        }
        this.transform.localRotation= Quaternion.Euler(0, 45f, 0);
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if ((Time.timeSinceLevelLoad < (LastShotTime + CoolOffTime))){return;}

        if (_expanding)
        {
            counter++;
            if (counter > 120)
            {
                LastShotTime = Time.timeSinceLevelLoad;
                _expanding = false;
            }
        } else {
            counter--;
            if (counter < 1)
            {
                LastShotTime = Time.timeSinceLevelLoad;
                _expanding = true;
            }
        }
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                squares[x,y].transform.localPosition = new Vector3(
                    (( (-xDim/2f) + 0.5f) + x) * (xStep + (counter*0.01f)),
                    0f,
                    (( (-yDim/2f) + 0.5f) +  y) * (yStep +(counter*0.01f))
                );
            }
        }
    }
}
