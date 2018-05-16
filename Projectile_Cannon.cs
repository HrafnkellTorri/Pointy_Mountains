using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Cannon : MonoBehaviour {

    public GameObject explosion;

    public float projectileSpeed = -500; 

    Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 10);
        rb.AddForce(transform.up * projectileSpeed, ForceMode.Force);
    }

    void FixedUpdate ()
    { 

    }


    void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Water_Top")
        {
            rb.AddForce(Vector3.up * 1000);
        }
    }
}
