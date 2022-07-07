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

    private bool pickupAnimStart;
    private Vector3 travelDist;

    private GameObject player;

    void Start()
    {
        initPosY = transform.position.y;
        spinRate = 3f;
        xRot = 45f;
        yRot = 10f;
        bounceRate = 0.5f;
        bounceLimit = 0.5f;
        bounceHeight = initPosY;

        pickupAnimStart = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (pickupAnimStart)
        {
            transform.localScale = transform.localScale * .99f;
            travelDist = Vector3.Lerp(gameObject.transform.parent.gameObject.transform.position, player.transform.position, 0.01f);
            spinRate = 20f;
            
            //Debug.Log(Vector3.Distance(transform.position, travelDist));
            if(Vector3.Distance(transform.position, travelDist) < 0.07f) 
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
        pickupAnimStart = true;
        
    }
}
