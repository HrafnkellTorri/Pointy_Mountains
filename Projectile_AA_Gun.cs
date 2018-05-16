using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_AA_Gun : MonoBehaviour {

    public AudioClip impact;
    public AudioClip impact2;
    AudioSource audioSource;

    public GameObject explosion;
    public GameObject destroyprojectile;
    public float projectileSpeed = 5;
    public float distance;
    public int soundselection;

    Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        transform.LookAt(Flight.playerlocation + new Vector3(UnityEngine.Random.Range(-4,4), UnityEngine.Random.Range(-4, 4), UnityEngine.Random.Range(-4, 4)));
        Invoke("AA_Burst", UnityEngine.Random.Range(3,6));
        Destroy(gameObject, 10f);
        soundselection = UnityEngine.Random.Range(0, 10);
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void LateUpdate()
    {


        distance = Vector3.Distance(transform.position, Flight.playerlocation);


        if (distance < 10)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(destroyprojectile, 4);
        }

        transform.Translate(Vector3.forward * projectileSpeed );

    }

    void OnCollisionEnter(Collision collision)
    {
        AA_Burst();
        Destroy(gameObject);

    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    public void AA_Burst()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        projectileSpeed = 0;
        
        if (soundselection != 5)
        {
            audioSource.PlayOneShot(impact, 0.1F);
        }
        else if (soundselection == 5)
        {
            audioSource.PlayOneShot(impact2, 0.1F);
            //Debug.Log("Hljóð 5");
        }
        Destroy(destroyprojectile);
        Destroy(gameObject, 2.5f);
    }
}
