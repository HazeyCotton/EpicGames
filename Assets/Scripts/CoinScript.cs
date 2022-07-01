using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class CoinScript: MonoBehaviour
{

    public float spinRate;
    public float xRot;
    public float yRot;
    public float zRot;

    void Start()
    {
        spinRate = 3f;
        xRot = 45f;
        yRot = 10f;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(xRot,yRot,zRot) * (spinRate * Time.deltaTime));
    }
}
