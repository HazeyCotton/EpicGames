using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public float positionSmoothTime = 1f;		// a public variable to adjust smoothing of camera motion
    public float rotationSmoothTime = 1f;
    public float positionMaxSpeed = 50f;        //max speed camera can move
    public float rotationMaxSpeed = 50f;
	public Transform desiredPose;			// the desired pose for the camera, specified by a transform in the game
    public Transform target;
	
    protected Vector3 currentPositionCorrectionVelocity;
    protected Quaternion quaternionDeriv;

    protected float angle;

    public bool Falling = false;
    
	void LateUpdate ()
	{
        
        if (desiredPose != null)
        {
            if(!Falling)
            {
                transform.position = Vector3.SmoothDamp(transform.position, desiredPose.position, ref currentPositionCorrectionVelocity, positionSmoothTime, positionMaxSpeed, Time.deltaTime);

                var targForward = desiredPose.forward;

                var targetRotation = Quaternion.LookRotation(targForward, Vector3.up);
                targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, target.rotation.eulerAngles.z);
                transform.rotation = QuaternionUtil.SmoothDamp(transform.rotation, targetRotation, ref quaternionDeriv, rotationSmoothTime);
                
            } else {
                Vector3 _direction = (target.position - transform.position).normalized;
 
            //create the rotation we need to be in to look at the target
                Quaternion _lookRotation = Quaternion.LookRotation(_direction);

                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime );
        
            }
        }
    }
}

