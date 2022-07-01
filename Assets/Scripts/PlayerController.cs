using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public float acceleration = 1.1f;
    public float deceleration = 1.4f;
    public float maxSpeed = 2;
    public float rotationSpeed = 2.5f;
    public float rotationDeceleration = 0.1f;
    public float maxVerticalTilt = 0.15f;
    public float maxHorizontalTilt = 0.3f;

    public TextMeshProUGUI timerLabel;
    public TextMeshProUGUI livesLabel;

    public GameObject finishLabelObject;

    private float propulsion;
    private float propulsionSum;
    private float rotation;
    private Vector3 rotationSum;

    private Rigidbody rb;
    private float time;
    private bool reachedFlag;

    public Vector3 _lastCheckpointPos;

    public int deathCount;

    void Awake()
    {
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
        if (speed > 10)
            speed -= 1 * Time.deltaTime;

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
        rb.AddForce(new Vector3(Mathf.Sin(rotationSum.y), 0.0f, Mathf.Cos(rotationSum.y)) * propulsionSum * speed);

        // Timer update
        if (!reachedFlag)
            time += Time.deltaTime;

        SetTimerLabel();
        SetLivesLabel();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SpeedBoost"))
        {
            other.gameObject.SetActive(false);
            speed += 2;
        } else if (other.gameObject.CompareTag("Finish"))
        {
            finishLabelObject.SetActive(true);
            reachedFlag = true;
        }
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
        int lives = 3 - deathCount;
        if (lives != 1)
        {
            livesLabel.text = lives.ToString() + " Lives Left";
        } else {
            livesLabel.text = lives.ToString() + " Life Left";
        }
    }

    public Vector3 GetRotation() {
        return rotationSum;
    }

    void OnCollsionenter()
    {

    }

}
