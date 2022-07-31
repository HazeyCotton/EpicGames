using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillLogic : MonoBehaviour
{
    private int Dethklok = 0;
    private bool Falling = false;

    public Camera playerCam;

    private PlayerController playerPC;
    private Rigidbody playerRb;

    void FixedUpdate()
    {
        
        
        if (Falling)
        {
            
            Dethklok++;
            if (Dethklok > 30)
            {
                playerRb.Sleep();
                playerPC.transform.position = playerPC._lastCheckpointPos;


                if (SceneManager.GetActiveScene().name != "Level_0")
                {
                    playerPC.deathCount++;
                }
                

                playerRb.velocity = Vector3.zero;

                playerPC.turnToTarget(playerPC._lastCheckpointLookAt);
                playerRb.WakeUp();
                

                if (playerPC.deathCount >= 3)
                {
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
        if (c.attachedRigidbody != null)
        {
            if (c.gameObject.tag == "Projectile") 
            {
                c.gameObject.transform.parent.gameObject.GetComponent<Projectile>().AcceptHit();
                return;
            }
            PlayerController pc = c.attachedRigidbody.gameObject.GetComponent<PlayerController>();
            Rigidbody rb = c.attachedRigidbody.gameObject.GetComponent<Rigidbody>();
            if (c.gameObject.tag == "Player" && rb != null)
            {
                playerPC = pc;
                playerRb = rb;
                Dethklok = 0;
                FindObjectOfType<AudioManager>().Play("PlayerDeath");
                startDeathFall();
            }
        }
    }

    void startDeathFall()
    {
        Falling = true;
        playerCam.GetComponent<CameraController>().Falling = true;
    }
}