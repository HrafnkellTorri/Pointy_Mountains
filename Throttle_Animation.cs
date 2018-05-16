using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throttle_Animation : MonoBehaviour {

    public GameObject throttle;
    public float Fthrottle;
    public float Pos;
    public Vector3 CurrPos;

    void Start ()
    {

    }
	
	void Update ()
    {
        CurrPos = new Vector3(throttle.transform.position.x, Pos = throttle.transform.position.y + Fthrottle/50, throttle.transform.position.z);
        throttle.transform.position = CurrPos;
    }
}
