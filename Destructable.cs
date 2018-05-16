using System;
using UnityEngine;

public class Destructable : MonoBehaviour {

    public GameObject destroyedversion;
    public GameObject Alive_Indicator;
    public bool isRewarded = true;
    public float health = 50f;


    private void Update()
    {


    }

    public void OnCollisionEnter(Collision collision)
    {   

        if(collision.gameObject.tag == "Player_Projectile")
        {
            health -= GunCannon.gunDamage;
            if (health <= 0)
            {
                if (isRewarded == true)
                {
                    Point_Counter.TargetIsDestroyed = true;
                }
                Instantiate(destroyedversion, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }

    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player_Projectile")
        {
            Instantiate(destroyedversion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
