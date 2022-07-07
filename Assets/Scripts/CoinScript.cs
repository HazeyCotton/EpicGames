using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class CoinScript: MonoBehaviour
{

    public float spinRate;
    public float xRot;
    public float yRot;
    public float zRot;

    public float bounceRate;
    public float bounceLimit;

    private float bounceHeight;
    private bool goingUp;
    private float initPosY;

    // Making bool for if the object has been picked up so it doesn't double count
    public bool pickedUp;

    private bool pickupAnimStart;
    private Vector3 travelDist;

    private GameObject player;
    public int NUM_FRAMES_BEFORE_DELETE;

    private int delFrameCounter;

    void Start()
    {
        delFrameCounter = 0;
        pickedUp = false;
        initPosY = transform.position.y;
        spinRate = 3f;
        xRot = 45f;
        yRot = 10f;
        bounceRate = 0.5f;
        bounceLimit = 0.5f;
        bounceHeight = initPosY;
        NUM_FRAMES_BEFORE_DELETE = 15;
        pickupAnimStart = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (pickupAnimStart)
        {
            delFrameCounter++;
            transform.localScale = transform.localScale * .95f;
            travelDist = ((4*gameObject.transform.parent.gameObject.transform.position) +  player.transform.position)/5;
            spinRate = 20f;
            
            //Debug.Log(Vector3.Distance(transform.position, travelDist));
            if(delFrameCounter > NUM_FRAMES_BEFORE_DELETE) 
            {
                pickupAnimStart = false;
                gameObject.SetActive(false);
            }
            gameObject.transform.parent.gameObject.transform.position = travelDist;

        }

        if ((bounceHeight-initPosY) > bounceLimit) 
        {
            goingUp = false;
        } else if ((bounceHeight-initPosY) < 0)
        {
            goingUp = true;
        }

        if (goingUp) 
        {
            bounceHeight += (bounceRate * Time.deltaTime);
        } else {
            bounceHeight -= (bounceRate * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, bounceHeight, transform.position.z);
        transform.Rotate(new Vector3(xRot,yRot,zRot) * (spinRate * Time.deltaTime));
    }


    void OnTriggerEnter(Collider c)
    {
        // If we collide with something other than a player return
        if (!c.gameObject.CompareTag("Player")){return;}
        player = c.gameObject;
        if (!pickedUp) {
            player.GetComponent<PlayerController>().points++;
        }
        pickedUp = true;
        pickupAnimStart = true;
        
    }
}
