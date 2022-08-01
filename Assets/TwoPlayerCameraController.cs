using UnityEngine;
using System.Collections;

public class TwoPlayerCameraController : MonoBehaviour
{
    public float positionSmoothTime = 1f;		// a public variable to adjust smoothing of camera motion
    public float rotationSmoothTime = 1f;
    public float positionMaxSpeed = 50f;        //max speed camera can move
    public float rotationMaxSpeed = 50f;
    public Transform playerPose;
    public Transform strangerPose;
    private Vector3 desiredPose;			// the desired pose for the camera, specified by a transform in the game
    public Transform target;

    protected Vector3 currentPositionCorrectionVelocity;
    protected Quaternion quaternionDeriv;

    protected float angle;

    public bool Falling = false;

    private float minDistanceForZoom = 10f;
    private float maxPossibleDistance = 50f;
    private float minY = 10f;
    private float maxY = 50f;

    //private void Start()
    //{
        
    //    //desiredPose = Vector3.Cross(playerPose.position, strangerPose.position).normalized;
    //    //desiredPose = desiredPose.normalized;
    //    desiredPose = playerPose.position;
    //}


    void LateUpdate()
    {
        Move();
        Zoom();
        
        ////desiredPose = playerPose.position;
        ////desiredPose = Vector3.Cross(playerPose.position, strangerPose.position).normalized;
        ////desiredPose = desiredPose.normalized;


        //if (desiredPose != null) {
        //    if (!Falling) {
        //        transform.position = Vector3.SmoothDamp(transform.position, desiredPose, ref currentPositionCorrectionVelocity, positionSmoothTime, positionMaxSpeed, Time.deltaTime);

        //        var targForward = playerPose.forward;

        //        var targetRotation = Quaternion.LookRotation(targForward, Vector3.up);
        //        targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, target.rotation.eulerAngles.z);
        //        transform.rotation = QuaternionUtil.SmoothDamp(transform.rotation, targetRotation, ref quaternionDeriv, rotationSmoothTime);

        //    } else {
        //        Vector3 _direction = (target.position - transform.position).normalized;

        //        //create the rotation we need to be in to look at the target
        //        Quaternion _lookRotation = Quaternion.LookRotation(_direction);

        //        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime);

        //    }
        //}
    }

    private void Move() {
        desiredPose = GetDesiredPose();
        transform.position = Vector3.SmoothDamp(transform.position, desiredPose, 
            ref currentPositionCorrectionVelocity, positionSmoothTime, positionMaxSpeed, Time.deltaTime);
    }

    private void Zoom() {

        Bounds bound = CoverPlayers();
        float distance;
        if (bound.size.x > bound.size.z) {
            distance = bound.size.x;
        } else {
            distance = bound.size.z;
        }

        if (distance < minDistanceForZoom) {
            distance = 0f;
        }

        float newY = Mathf.Lerp(minY, maxY, distance / maxPossibleDistance);

        transform.position = new Vector3(transform.position.x,
            Mathf.Lerp(transform.position.y, newY, Time.deltaTime),
            transform.position.z);
    }

    private Vector3 GetDesiredPose() {
        Bounds bounding = CoverPlayers();

        Vector3 desired = bounding.center;

        return desired;
    }

    private Bounds CoverPlayers() {
        Bounds bounds = new Bounds(playerPose.position, Vector3.zero);
        bounds.Encapsulate(strangerPose.position);

        return bounds;
    }
}