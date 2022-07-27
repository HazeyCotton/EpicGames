using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

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
    public static int points = 0;

    public TextMeshProUGUI timerLabel;
    public TextMeshProUGUI livesLabel;
    public TextMeshProUGUI scoreLabel;

    public GameObject finishLabelObject;

    private float propulsion;
    private float propulsionSum;
    private float rotation;
    private Vector3 rotationSum;

    private Rigidbody rb;
    public static float time;
    private bool reachedFlag;

    public Vector3 _lastCheckpointPos;
    public Vector3 _lastCheckpointLookAt;

    public int deathCount;

    AudioSource audioSource;
    float engineVolume;
    float oldPropulsion;

    void Start()
    {
         _lastCheckpointLookAt = new Vector3(this.transform.position.x +(5f * Mathf.Sin(this.transform.rotation.y/180f*Mathf.PI)),
                                        this.transform.position.y,
                                            this.transform.position.z +(5f * Mathf.Cos(this.transform.rotation.y/180f*Mathf.PI))); 
       /* Debug.Log(this.gameObject.transform.position);
        Debug.Log(_lastCheckpointLookAt);
        Debug.Log(this.transform.rotation.y);
        Debug.Log(5f * Mathf.Cos(this.transform.rotation.y/180f*Mathf.PI));
        Debug.Log(5f * Mathf.Sin(this.transform.rotation.y/180f*Mathf.PI));*/


    }
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
        Debug.Log(movementVector);
        propulsion = movementVector.y;
        rotation = movementVector.x;
    }

    void FixedUpdate()
    {
        // Timer update
        if (!reachedFlag)
            time += Time.deltaTime;

        SetTimerLabel();
        SetLivesLabel();
        SetScoreLabel();

        // Speed limitation
        //if (speed > 10)
            //speed -= 1 * Time.deltaTime;

        // Propulsion control
        if (propulsion == 0) {
            rb.velocity *= 1f-deceleration;
            //Debug.Log(rb.velocity);
            propulsionSum *= 1f-deceleration;
            rotationSum.x *= 1f-rotationDeceleration;
        } else {
            propulsionSum = propulsion*acceleration*Time.deltaTime;
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

        rb.velocity += new Vector3(Mathf.Sin(rotationSum.y), 0f, Mathf.Cos(rotationSum.y)) * propulsionSum * speed;
        
        
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
            Scene scene = SceneManager.GetActiveScene();

            if (scene.name == "Level_1")
            {
                SceneManager.LoadScene("Level_2");
            } else if (scene.name == "Level_2") {
                SceneManager.LoadScene("Level_3");

            } else if (scene.name == "Level_3")
            {
                SceneManager.LoadScene("Level_4");
            } else if (scene.name == "Level_4")
            {
                SceneManager.LoadScene("Level_5");
            } else if (scene.name == "Level_5"){
                SceneManager.LoadScene("WinScene");
            }
            
        }
        /*
        else if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            points++;
        }*/

        
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
        int lives = 3 - deathCount;
        if (lives != 1)
        {
            livesLabel.text = lives.ToString() + " Lives Left";
        } else {
            livesLabel.text = lives.ToString() + " Life Left";
        }

        if (lives == 0) {
             SceneManager.LoadScene("LoseScene");
        }
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
        //Debug.Log(targetPos);
        var targetPos2d = new Vector2(targetPos.x, targetPos.z);
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
        
        //Debug.Log(target);
       // Debug.Log(this.gameObject.transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
     
        rb.MoveRotation(lookRotation);
        rotationSum = (lookRotation.eulerAngles/180f*Mathf.PI);
        //Debug.Log(rotationSum);
    }

 

}
