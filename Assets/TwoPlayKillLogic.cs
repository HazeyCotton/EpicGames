using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TwoPlayKillLogic : MonoBehaviour
{
    private int Dethklok = 0;
    private bool Falling = false;

    public Camera playerCam;

    public GameObject player;
    public GameObject stranger;

    private PlayerController playerPC;
    private Rigidbody playerRb;
    private Vector3 playerStartingPosition;

    private PlayerController strangerPC;
    private Rigidbody strangerRb;
    private Vector3 strangerStartingPosition;

    private void Start()
    {
        playerPC = player.GetComponent<PlayerController>();
        playerRb = player.GetComponent<Rigidbody>();
        playerStartingPosition = player.transform.position;

        strangerPC = stranger.GetComponent<PlayerController>();
        strangerRb = stranger.GetComponent<Rigidbody>();
        strangerStartingPosition = stranger.transform.position;


    }

    void FixedUpdate()
    {
        if (Falling) {

            Dethklok++;
            if (Dethklok > 30) {
                playerRb.Sleep();
                playerPC.transform.position = playerStartingPosition;

                strangerRb.Sleep();
                strangerPC.transform.position = strangerStartingPosition;
                
                if (SceneManager.GetActiveScene().name != "Level_0") {
                    playerPC.deathCount++;
                }


                playerRb.velocity = Vector3.zero;

                strangerRb.velocity = Vector3.zero;

                playerPC.turnToTarget(playerStartingPosition);
                playerRb.WakeUp();

                strangerPC.turnToTarget(strangerStartingPosition);
                strangerRb.WakeUp();


                Debug.Log("deathcount: " + (playerPC.deathCount + strangerPC.deathCount));

                if (playerPC.deathCount + strangerPC.deathCount >= 3) {
                    SceneManager.LoadScene("LoseScene");
                    Time.timeScale = 1f;
                }

                Falling = false;
                playerCam.GetComponent<CameraController>().Falling = false;
            }
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody != null) {
            if (c.gameObject.tag == "Projectile") {
                c.gameObject.transform.parent.gameObject.GetComponent<Projectile>().AcceptHit();
                return;
            }



            if (c.attachedRigidbody.gameObject == player) {
                PlayerController pc = c.attachedRigidbody.gameObject.GetComponent<PlayerController>();
                Rigidbody rb = c.attachedRigidbody.gameObject.GetComponent<Rigidbody>();

                if (c.gameObject.tag == "Player" && rb != null) {
                    playerPC = pc;
                    playerRb = rb;

                    //strangerPC = spc;
                    //strangerRb = srb;

                    Dethklok = 0;
                    FindObjectOfType<AudioManager>().Play("PlayerDeath");
                    startDeathFall();
                }

            } else if (c.attachedRigidbody.gameObject == stranger) {
                PlayerController spc = c.attachedRigidbody.gameObject.GetComponent<PlayerController>();
                Rigidbody srb = c.attachedRigidbody.gameObject.GetComponent<Rigidbody>();



                if (c.gameObject.tag == "Player" && srb != null) {
                    strangerPC = spc;
                    strangerRb = srb;

                    //strangerPC = spc;
                    //strangerRb = srb;

                    Dethklok = 0;
                    FindObjectOfType<AudioManager>().Play("PlayerDeath");
                    startDeathFall();
                }
            }
            
            //PlayerController spc = c.attachedRigidbody.gameObject.GetComponent<PlayerController>();
            //Rigidbody srb = c.attachedRigidbody.gameObject.GetComponent<Rigidbody>();
            
        }
    }

    void startDeathFall()
    {
        Falling = true;
        playerCam.GetComponent<CameraController>().Falling = true;
    }
}
