using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowarsPlayer_X_AXIS : MonoBehaviour
{
    //public float speed;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Flight.playerlocation + new Vector3(5, -5, 2));
        transform.Rotate(- 90, -90, transform.eulerAngles.z);
    }
}
