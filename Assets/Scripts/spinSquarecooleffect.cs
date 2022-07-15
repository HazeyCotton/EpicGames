using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinningSquareCool: MonoBehaviour, IObstacle
{

    public spinningSquare spinningSquarePrefab;

    public int numSquares;

    private GameObject[] squares;

    public float animationSpeed
    {
        get;
        set;
    }

    // Start is called before the first frame update
    void Start()
    {
        animationSpeed = 1f;
        numSquares = 20;

        squares = new GameObject[numSquares];

        for (int x = 0; x < numSquares; x++)
        {
            
            var sqr = Instantiate<spinningSquare>(spinningSquarePrefab);
            sqr.transform.parent = this.gameObject.transform;

            sqr.transform.localPosition = new Vector3(
                0f,
                0f,
                0.5f * x);

            squares[x] = sqr.gameObject;
        
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int x = 0; x < numSquares; x++)
        {
            
            squares[x].transform.localRotation = Quaternion.Euler(
                squares[x].transform.localRotation.x,
                squares[x].transform.localRotation.y,
                squares[x].transform.localRotation.z +3f);
        
        }
    }
}
