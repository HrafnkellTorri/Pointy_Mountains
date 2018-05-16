using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AA_Gun_Ship : MonoBehaviour {

    public GameObject projectile_AA_Gun;

    public float timeBetweenShots = 0.2f;
    public float distance;

    // Use this for initialization
    void Start ()
    {
       InvokeRepeating("Spawn", .01f, timeBetweenShots + Random.Range(0,2));
    }

    void FixedUpdate()
    {
        distance = Vector3.Distance(transform.position, Flight.playerlocation);
    }

    void Spawn ()
    {

        if (distance > 150 && distance < 40000)
        {
            Instantiate(projectile_AA_Gun, transform.position, Quaternion.identity);
        }
    }
}
