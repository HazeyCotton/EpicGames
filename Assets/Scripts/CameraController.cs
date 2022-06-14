using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset;
    private float tiltX;
    private float tiltZ;

    private float maxTilt = 45;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
        tiltX = transform.rotation.x - player.transform.rotation.x;
        tiltZ = transform.rotation.z - player.transform.rotation.z;
    }

    void FixedUpdate()
    {
        
    }

    void Update()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;

        transform.rotation = Quaternion.Euler(player.transform.rotation.x + tiltX, 0.0f, player.transform.rotation.z + tiltZ);
    }
}
