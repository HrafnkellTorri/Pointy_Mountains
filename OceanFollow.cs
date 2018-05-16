using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanFollow : MonoBehaviour {

    public Transform Player;
    public float MoveRate;

    private void Start()
    {
        InvokeRepeating("MoveBasePlate", 2.0f, MoveRate);
    }

    void MoveBasePlate()
    {
        gameObject.transform.position = new Vector3(Player.transform.position.x, gameObject.transform.position.y, Player.transform.position.z);
    }
}
