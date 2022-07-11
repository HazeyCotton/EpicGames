using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAnimationSpeed : MonoBehaviour
{
    public Animator[] animatorControlledObstacles;

    private float animationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        animationSpeed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setAnimationSpeed(float newSpeed)
    {
        animationSpeed = newSpeed;
        foreach (Animator anim in animatorControlledObstacles)
        {
            anim.speed = animationSpeed;
        }
    }
}
