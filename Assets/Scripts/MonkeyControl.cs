using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyControl : MonoBehaviour
{
    public GameObject player;
    private PlayerController ctrl;

    void Awake()
    {
        ctrl = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(
            ctrl.GetRotation().x / Mathf.PI * 180, 
            ctrl.GetRotation().y / Mathf.PI * 180, 
            ctrl.GetRotation().z / Mathf.PI * 180);
    }
}
