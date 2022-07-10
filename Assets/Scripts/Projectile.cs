using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public Rigidbody rbody {get; private set;}

    public ProjectileThrower mgr;

    private int delFrameCounter;

    private bool collided;

    public int despawnTimeInFrames;

    void Awake()
    {
        collided = false;
        delFrameCounter = 0;

        rbody = GetComponent<Rigidbody>();

        if(rbody == null)
        {
            Debug.LogError("no rigid body");
        }
    }

    public void AcceptHit()
    {
        //Debug.Log("Collision");
        collided = true;
        
    }

    void OnEnable()
    {
        collided = false;
        delFrameCounter = 0;
    }

    void FixedUpdate()
    {
        if (collided) 
        {
            delFrameCounter++;
            if (delFrameCounter > despawnTimeInFrames)
            {
                this.gameObject.SetActive(false);

                if (mgr != null)
                {
                    mgr.Recycle(this);
                }
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        AcceptHit();
    }

}
