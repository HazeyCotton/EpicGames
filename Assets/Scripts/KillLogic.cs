using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillLogic : MonoBehaviour
{
    void OnTriggerEnter(Collider c) 
    {
        if (c.attachedRigidbody != null) {
            PlayerController pc = c.attachedRigidbody.gameObject.GetComponent<PlayerController>();
            Rigidbody rb = c.attachedRigidbody.gameObject.GetComponent<Rigidbody>();
            if (pc != null && rb != null) 
            {
                pc.transform.position = pc._lastCheckpointPos;

                pc.deathCount++;

                rb.velocity = Vector3.zero;

                if (pc.deathCount >= 3)
                {
                    SceneManager.LoadScene("StartScreen");
                    Time.timeScale = 1f; 
                }

            } 
        }
        
        
    }
}
