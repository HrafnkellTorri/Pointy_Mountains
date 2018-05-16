using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCannon : MonoBehaviour {

    public Camera GunPosition;
    public AudioSource GunSounds;
    public AudioClip myclip;
    public GameObject impacteffect;
    public ParticleSystem GunFlash;
    public GameObject bullet;
    public GameObject bulletPOS;

    public static float gunDamage = 25;
    public float GunCoolDown = 2f;


    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            if (GunCoolDown <= Time.time)
            {
                GunFlash.Play();
                ShootGun();
                GunSounds.PlayOneShot(myclip, 0.3f);
                GunCoolDown = Time.time + 2f;
            }
        }
    }

    void ShootGun()
    {

        Instantiate(bullet, bulletPOS.transform.position, bulletPOS.transform.rotation);

    }
}
