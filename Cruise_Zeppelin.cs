using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cruise_Zeppelin : MonoBehaviour {

    public float speed = 20; // Adjust to make your NPC move however fast you want.
    public bool alive = true;

    void FixedUpdate()
    {
        if (alive == true)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            transform.Rotate(Vector3.up * Time.deltaTime, Space.World);

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(alive);
        alive = false;
        Destroy(gameObject);
    }
}
