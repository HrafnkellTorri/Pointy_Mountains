using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cruise_Npc : MonoBehaviour
{

    public float speed = 20; // Adjust to make your NPC move however fast you want.
    public float distanceFromObsticle = 500;

    private Vector3 clearanceVector = new Vector3(0, 0, 0);
    public GameObject DeathIndicator;
    public GameObject Gunpos;
    public GameObject Newgun;
    public Transform NavSystem;

    public bool isRewarded = true;
    public bool isDead = false;
    public float health = 50f;

    private void Start()
    {
        GetComponent<Animator>().enabled = false;
    }

    private void Update()
    {
        if (isDead)
        {
            Destroy(DeathIndicator);
            GetComponent<Animator>().enabled = true;
            GetComponent<Animator>().Play("Destroyer_Sinking");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player_Projectile")
        {
            health -= GunCannon.gunDamage;
            if (health <= 0)
            {
                if (isRewarded == true && isDead == false)
                {
                    Point_Counter.TargetIsDestroyed = true;
                }
                isDead = true;
            }
        }
    }
}