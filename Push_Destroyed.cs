using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push_Destroyed : MonoBehaviour {

    Rigidbody rb;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
              		
	}
	
	// Update is called once per frame
	void Update () {
        rb.AddForce(-1 * 5, 0, 0);
        transform.Rotate(Vector3.down * 5 * Time.deltaTime);
    }
}
