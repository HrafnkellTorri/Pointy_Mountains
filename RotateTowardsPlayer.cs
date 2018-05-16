using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsPlayer : MonoBehaviour {

    public Camera cam;

    private void Update()
    {
        transform.LookAt(2 * transform.position - Camera.main.transform.position);
        transform.rotation = Camera.main.transform.rotation;
    }
}
