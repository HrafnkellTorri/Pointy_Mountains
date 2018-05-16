using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_MainTitles : MonoBehaviour {

    public string MissionMessage;
    public string ExtraMissionMessage;
    private int messagecount = 0;


    // Use this for initialization
    void Start ()
    {
        InvokeRepeating("IntroMessages", 5f, 10);
    }


    void IntroMessages()
    {
        TextMeshPro tm = GetComponent<TextMeshPro>();
        //Debug.Log("INtro");
        if (Flight.isAlive == false)
        {
            tm.SetText("GAME OVER");
        }
        else
        {
            if (messagecount == 0)
            {
                tm.SetText(MissionMessage);
                //Debug.Log("INtro 1");
            }
            else if (messagecount == 1)
            {
                tm.SetText(ExtraMissionMessage);
            }
            else if (Flight.isAlive == false)
            {
                tm.faceColor = new Color32(255, 0, 0, 255);
                tm.SetText("GAME OVER");
            }
            else
            {
                tm.SetText("");
            }
        }
        messagecount++;
    }
}
