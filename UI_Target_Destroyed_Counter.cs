using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Target_Destroyed_Counter : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
       tm = GetComponent<TextMeshPro>();
    }

    TextMeshPro tm;

    // Update is called once per frame
    void Update ()
    {
        tm.SetText("Targets Destroyed: " + (Point_Counter.TargetsDestroyedCounter).ToString());
	}
}
