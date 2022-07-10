using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    private Vector3 lookAtPosition;

    // Start is called before the first frame update
    void Start()
    {
        lookAtPosition = new Vector3(this.transform.position.x +(Mathf.Cos(this.transform.rotation.eulerAngles.y/180f*Mathf.PI)),
                                        this.transform.position.y,
                                            this.transform.position.z - (Mathf.Sin(this.transform.rotation.eulerAngles.y/180f*Mathf.PI)));   
        /*
        Debug.Log(this.gameObject.transform.position);
        Debug.Log(lookAtPosition);
        Debug.Log(this.transform.rotation.eulerAngles.y);
        Debug.Log(5f * Mathf.Cos(this.transform.rotation.eulerAngles.y/180f*Mathf.PI));
        Debug.Log(5f * Mathf.Sin(this.transform.rotation.eulerAngles.y/180f*Mathf.PI));*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody != null)
        {
            PlayerController pc = c.attachedRigidbody.gameObject.GetComponent<PlayerController>();
            Rigidbody rb = c.attachedRigidbody.gameObject.GetComponent<Rigidbody>();
            if (pc != null && rb != null)
            {
                pc._lastCheckpointPos = this.gameObject.transform.position;
                pc._lastCheckpointLookAt = lookAtPosition;

                this.gameObject.SetActive(false);
            }
        }
    }
}
