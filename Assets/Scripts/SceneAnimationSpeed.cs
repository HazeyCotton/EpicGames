using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAnimationSpeed : MonoBehaviour
{
    public Animator[] animatorControlledObstacles;

    public GameObject[] movingObstacles;

    private float animationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        animationSpeed = 1f;

        movingObstacles = GameObject.FindGameObjectsWithTag("ObstacleOrRailing");

        if (movingObstacles.Length == 0)
        {
            Debug.Log("No game objects are tagged with ObstacleOrRailing");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setAnimationSpeed(float newSpeed)
    {
        animationSpeed = newSpeed;
        
        // Current way game is setup has obstacles and railings tagged as the same
        // Will now go through extra things than just obstacles
        // Therefore slightly ineefecient but will have good error handling
        foreach (GameObject obj in movingObstacles)
        {
            SimpleObstacleController cont = obj.GetComponent<SimpleObstacleController>();
            if (cont != null)
            {
                cont.setRotationRate(animationSpeed);
                continue;
            }

            Animator anim = obj.GetComponent<Animator>();
            if (anim != null)
            {
                anim.speed = animationSpeed;
                continue;
            }

        }
    }
}
