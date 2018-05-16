using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_Sinking : MonoBehaviour {

    Rigidbody rb;
    public float sinkingSpeed = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 20);
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.up * sinkingSpeed);
        //rb.transform.Rotate(Vector3.up);
    }
}
