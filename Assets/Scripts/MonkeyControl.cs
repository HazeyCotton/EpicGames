using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MonkeyControl : MonoBehaviour
{
    public GameObject player;
    private PlayerController ctrl;

    private Animator anim;
    private Rigidbody rbody;

    public float animationSpeed = 1f;
    public float rootMovementSpeed = 1f;
    public float rootTurnSpeed = 1f;

    float _inputForward = 0f;
    float _inputTurn = 0f;

    Vector3 foxTransformPos;

    void Awake()
    {
        ctrl = player.GetComponent<PlayerController>();
        if (ctrl == null)
            Debug.Log("Player controller could not be found");

        anim = GetComponent<Animator>();
        if (anim == null)
            Debug.Log("Animator could not be found");

    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        _inputForward = movementVector.y;
        _inputTurn = movementVector.x;
        //Debug.Log(_inputForward);
    }

    void FixedUpdate()
    {
        
        anim.SetFloat("velx", _inputTurn);
        anim.SetFloat("vely", _inputForward);

        
        Vector3 rotControl = ctrl.GetRotation();
        transform.rotation = Quaternion.Euler(
            rotControl.x / Mathf.PI * 180, 
            rotControl.y / Mathf.PI * 180,
            rotControl.z / Mathf.PI * 180);

        transform.position = new Vector3(this.transform.parent.gameObject.transform.position.x + (0.15f * (Mathf.Sin(this.transform.rotation.eulerAngles.y/180f*Mathf.PI))),
                                        this.transform.parent.gameObject.transform.position.y - 0.35f,
                                            this.transform.parent.gameObject.transform.position.z + (0.15f*(Mathf.Cos(this.transform.rotation.eulerAngles.y/180f*Mathf.PI))));   
        
    }
}
