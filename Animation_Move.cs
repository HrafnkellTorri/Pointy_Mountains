using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Move : MonoBehaviour {

    public Animator anim;
    public bool geardown = true;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            anim.Play("LeftWheel");
            geardown = false;
        }
        else if(Input.GetKeyDown(KeyCode.G))
        {
            anim.Play("LeftWheel_DOWN");
        }
	}
}
