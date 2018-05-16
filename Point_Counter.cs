using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point_Counter : MonoBehaviour
{
    public static int TargetsDestroyedCounter = 0;
    public AudioClip myclip;
    public static bool TargetIsDestroyed = false;

    AudioSource audioSource;

    private void Start()
    {
        TargetsDestroyedCounter = 0;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update ()
    {
		if(TargetIsDestroyed == true)
        {
            audioSource.PlayOneShot(myclip,2f);
            TargetsDestroyedCounter++;
            TargetIsDestroyed = false;
        }
	}
}
