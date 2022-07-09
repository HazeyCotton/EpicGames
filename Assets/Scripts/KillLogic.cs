using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillLogic : MonoBehaviour
{
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
            if (pc != null && rb != null)
            {
                FindObjectOfType<AudioManager>().Play("PlayerDeath");

                rb.Sleep();
                pc.transform.position = pc._lastCheckpointPos;
                
                pc.deathCount++;

                rb.velocity = Vector3.zero;

                pc.turnToTarget(pc._lastCheckpointLookAt);
                rb.WakeUp();

                if (pc.deathCount >= 3)
                {
                    SceneManager.LoadScene("StartScreen");
                    Time.timeScale = 1f;
                }

            }
        }


    }
}