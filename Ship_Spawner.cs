using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_Spawner : MonoBehaviour
{

    public GameObject Enemy;
    public int startSpawner;
    public int Interval1;
    public int Interval2;

	// Use this for initialization
	void Start ()
    {
        InvokeRepeating("SpawnEnemy", startSpawner, Random.Range(Interval1, Interval2));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SpawnEnemy()
    {
        Instantiate(Enemy, gameObject.transform.position + new Vector3(Random.Range(0, 400), gameObject.transform.position.y + 28, Random.Range(0, 300)), gameObject.transform.rotation);
    }
}
