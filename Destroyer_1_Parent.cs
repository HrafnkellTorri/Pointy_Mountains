using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer_1_Parent : MonoBehaviour
{

    public float speed = 20; // Adjust to make your NPC move however fast you want.
    public float distanceFromObsticle = 500;
    public float turnRate = 10;


    private Vector3 clearanceVector = new Vector3(0, 0, 0);
    public GameObject DeathIndicator;

    public Transform NavSystem;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(DeathIndicator)
        {
            CollisionAvertion();
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else if(!DeathIndicator)
        {
            if (speed > 0.1f)
            {
                speed -= 0.1f;
            }
            Destroy(gameObject, 50);
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            transform.Translate(Vector3.forward * -25 * Time.deltaTime);
        }
    }

    void CollisionAvertion()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer 
        if (Physics.Raycast(NavSystem.position + clearanceVector, transform.TransformDirection(Vector3.right) * distanceFromObsticle, out hit, Mathf.Infinity))
        {
            if (hit.distance < 1000)
            {
                Debug.DrawRay(NavSystem.position + clearanceVector, transform.TransformDirection(Vector3.right) * 1000, Color.yellow);
                //Debug.Log("Did Hit" + hit.distance);
                transform.Rotate(Vector3.up * turnRate * Time.deltaTime, Space.World);
            }
        }
        else
        {
            Debug.DrawRay(NavSystem.position + clearanceVector, transform.TransformDirection(Vector3.right) * 1000, Color.white);
            //Debug.Log("Did not Hit" + hit.distance);
            transform.Rotate(Vector3.up * 0 * Time.deltaTime, Space.World);
        }
    }
}   
