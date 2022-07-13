using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_self : MonoBehaviour
{
    public float speed;
    public float x = -1;
    public float y = -1;
    public float z = -1;


    // Start is called before the first frame update
    void Start()
    {

    }

    float num = 0;
    // Update is called once per frame
    void Update()
    {
        num += Time.deltaTime * this.speed;
        if (num > 360)
            num = 0;


        if (this.x == -1)
            this.transform.rotation = Quaternion.Euler(num, this.y, this.z);
        if (this.y == -1)
            this.transform.rotation = Quaternion.Euler(this.x, num, this.z);
        if (this.z == -1)
            this.transform.rotation = Quaternion.Euler(this.x, this.y, num);
    }
}
