using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI timerLabel;
    public GameObject finishLabelObject;

    private Rigidbody rb;
    private float time;
    private float movementX;
    private float movementY;
    private bool reachedFlag;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        time = 0;
        finishLabelObject.SetActive(false);
        reachedFlag = false;

        SetTimerLabel();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void FixedUpdate()
    {
        if (speed > 10)
        {
            speed -= 1 * Time.deltaTime;
        }

        rb.AddForce(new Vector3(movementX, 0.0f, movementY) * speed);

        if (!reachedFlag)
        {
            time += Time.deltaTime;
        }

        SetTimerLabel();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SpeedBoost"))
        {
            other.gameObject.SetActive(false);
            speed += 10;
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
}
