using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerControllerFlight : MonoBehaviour
{
    public float speed = 0;
    public float acceleration = 1.1f;
    public float deceleration = 1.4f;
    public float maxSpeed = 2;
    public float rotationSpeed = 2.5f;
    public float rotationDeceleration = 0.1f;
    public float maxVerticalTilt = 0.15f;
    public float maxHorizontalTilt = 0.3f;
    public int points = 0;

    public TextMeshProUGUI timerLabel;
    public TextMeshProUGUI livesLabel;
    public TextMeshProUGUI scoreLabel;

    public GameObject finishLabelObject;

    private float propulsion;
    private float propulsionSum;
    private float rotation;
    private Vector3 rotationSum;

    private Rigidbody rb;
    private float time;
    private bool reachedFlag;

    public Vector3 _lastCheckpointPos;
    public Quaternion _lastCheckpointRot;

    public int deathCount;

    AudioSource audioSource;
    float engineVolume;
    float oldPropulsion;

    void Awake()
    {
        engineVolume = 0f;
        audioSource = GetComponent<AudioSource>();


        _lastCheckpointPos = transform.position;

        rb = GetComponent<Rigidbody>();

        propulsion = 0;
        propulsionSum = 0;
        rotation = 0;
        rotationSum = new Vector3(0, 0, 0);

        time = 0;
        finishLabelObject.SetActive(false);
        reachedFlag = false;

        SetTimerLabel();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        propulsion = movementVector.y;
        rotation = movementVector.x;
    }

    void FixedUpdate()
    {
        // Speed limitation
        
        // Propulsion control
        if (propulsion == 0) {
            propulsionSum *= 1f-deceleration;
            rotationSum.x *= 1f-rotationDeceleration;
        } else {
            propulsionSum += propulsion*acceleration*Time.deltaTime;
            rotationSum.x += propulsion*rotationSpeed*Time.deltaTime;
        }

        // Propulsion limitation
        if (Mathf.Abs(propulsionSum) > maxSpeed)
            propulsionSum = propulsionSum > 0 ? maxSpeed : -maxSpeed;

        //audio stuff
        engineVolume = Mathf.Abs(propulsionSum / maxSpeed);
        audioSource.volume = engineVolume;


        // Rotation control
        if (rotation == 0) {
            rotationSum.z *= 1f-rotationDeceleration;
        } else {
            rotationSum.y += rotation*rotationSpeed*Time.deltaTime;
            rotationSum.z -= rotation*rotationSpeed*Time.deltaTime;
        }

        // Tilt limitation
        if (Mathf.Abs(rotationSum.x) > maxVerticalTilt)
            rotationSum.x = rotationSum.x > 0 ? maxVerticalTilt : -maxVerticalTilt;
        if (Mathf.Abs(rotationSum.z) > maxHorizontalTilt)
            rotationSum.z = rotationSum.z > 0 ? maxHorizontalTilt : -maxHorizontalTilt;

        // Apply force
        rb.AddForce(new Vector3(Mathf.Sin(rotationSum.y), Physics.gravity.y/10, Mathf.Cos(rotationSum.y)) * propulsionSum * speed);

        // Timer update
        if (!reachedFlag)
            time += Time.deltaTime;

        SetTimerLabel();
        SetLivesLabel();
        SetScoreLabel();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SpeedBoost"))
        {
            other.gameObject.SetActive(false);
            speed += 2;
        }
        else if (other.gameObject.CompareTag("Finish"))
        {
            finishLabelObject.SetActive(true);
            reachedFlag = true;
        }
        else if (other.gameObject.CompareTag("Coin"))
        {
            //other.gameObject.SetActive(false);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("ObstacleOrRailing"))
            FindObjectOfType<AudioManager>().Play("HitObject");
    }

    void SetTimerLabel()
    {
        var minutes = time / 60;
        var seconds = time % 60;
        var fraction = (time * 100) % 100;

        timerLabel.text = string.Format("{0:00} : {1:00} : {2:00}", minutes, seconds, fraction);
    }

    void SetLivesLabel()
    {
        
        livesLabel.text = Vector3.Magnitude(rb.velocity).ToString() + "m/s";
        
    }

    void SetScoreLabel()
    {
       
        scoreLabel.text = "Score: " + points.ToString();
        
    }

    public Vector3 GetRotation() {
        return rotationSum;
    }

    public void turnToTarget(Vector3 targetPos)
    {
        var targetPos2d = new Vector2(targetPos.x + 10f, targetPos.z);
        var minionPos2d = new Vector2(rb.transform.position.x, rb.transform.position.z);

        var minionToTarget2d = targetPos2d - minionPos2d;

        float distMinionFromTarget = minionToTarget2d.magnitude;

        var angle = Mathf.Atan2(0f, distMinionFromTarget);

        //Debug.Log($"Rotating by {angle * Mathf.Rad2Deg} degrees");

        float sin = Mathf.Sin(angle);
        float cos = Mathf.Cos(angle);

        // Rotate
        var newMinionToTarget2d = new Vector2(
            minionToTarget2d.x * cos - minionToTarget2d.y * sin,
            minionToTarget2d.x * sin + minionToTarget2d.y * cos);

        var adjTarg = new Vector3(minionPos2d.x + newMinionToTarget2d.x, targetPos.y, minionPos2d.y + newMinionToTarget2d.y);

        var target = adjTarg;

        Vector3 direction = (target - transform.position).normalized;
        Debug.Log(direction);
        //Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
        Quaternion lookRotation = Quaternion.FromToRotation(Vector3.zero, direction);
        //var canRot = TurnToFaceSpeedDegPerSec * Time.fixedDeltaTime;// Time.deltaTime;
        //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, canRot); //will be clamped if overshoots
        Debug.Log(lookRotation);
        rb.MoveRotation(lookRotation); //will be clamped if overshoots)
        rotationSum = new Vector3(lookRotation.x, lookRotation.y, lookRotation.z);
    }

 

}
