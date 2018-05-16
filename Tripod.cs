using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tripod : MonoBehaviour {

    public Transform playerTarget;

    public Transform vehReset;
    public Transform vehTarget;
    public GameObject canvas;

    Vector3 defaultDistance = new Vector3(0f, 20f, 0f);

    public float distancedamp = 45f;
    public float rotationaldamp = 10f;
    public bool inVehicle = true;
    public bool hasRotated = false;

    Vector3 defaultloc;

    void FixedUpdate ()
    {
        inVehicle = Flight.hasPilotStat;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (inVehicle == true)
            {
                //inVehicle = false;
            }
            else if (inVehicle == false)
            {
                inVehicle = true;
            }
        }


        if (inVehicle)
        {
            if(hasRotated == false)
            {
                hasRotated = true;
            }
            if (!vehTarget || !playerTarget)
            {
                gameObject.transform.Translate(new Vector3(0, 15, -3) * Time.deltaTime);
                Destroy(canvas);
                gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0, transform.eulerAngles.y, 0), 0.01f);
            }
            else
            {
                //target.transform.position = Random.insideUnitSphere * 1;
                //shake -= Time.deltaTime * 0.4;

                Vector3 toPos = vehTarget.position + (vehTarget.rotation * defaultDistance);
                Vector3 curPos = Vector3.Lerp(transform.position, toPos, distancedamp * Time.deltaTime);
                transform.position = curPos;

                Quaternion toRot = Quaternion.LookRotation(vehTarget.position - transform.position, vehTarget.right);
                Quaternion curRot = Quaternion.Slerp(transform.rotation, toRot, rotationaldamp * Time.deltaTime);
                transform.rotation = curRot;
            }
        }

    }
    private void Update()
    {
       

        if (!inVehicle)
        {
            transform.position = playerTarget.transform.position;
            transform.rotation = playerTarget.transform.rotation;
        }
        else
        {
            if (!playerTarget && !vehTarget)
            {

            }
            else
            {
                defaultloc = vehTarget.transform.position;

                //Look back
                if (Input.GetKeyUp(KeyCode.C))
                {
                    vehTarget.position = defaultloc + new Vector3(0, 0, 1175);
                    vehTarget.transform.position = vehReset.position;
                    vehTarget.Rotate(Vector3.right * 180);
                }

            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Destroy(transform);
    }
}
