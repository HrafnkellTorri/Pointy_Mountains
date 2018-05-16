using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cruise_Bomber_1 : MonoBehaviour {

    public float speed = 500; // Adjust to make your NPC move however fast you want.

    void FixedUpdate()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
    private void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        transform.Rotate(Vector3.right * -2);
    }
}
