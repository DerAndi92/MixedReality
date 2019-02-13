using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScaryAction : MonoBehaviour
{

    // Skript auf People Objekte setzen!

    private bool done = false; 

    void Start()
    {
        
    }

    // Wenn alle vier Bomben platziert wurden, rennen die ausgewählten Leute zum ausgewählten Ziel und verschwinden danach.
    void Update()
    {
        if(!done && GameController.Instance.eventBombPlaced == 4)
        {
            done = true;
            gameObject.GetComponent<Moveit>().run();
            gameObject.GetComponent<Moveit>().speed = 2.3f;
            gameObject.GetComponent<Waypoints>().repeat = false;
            gameObject.GetComponent<Waypoints>().vanish = true;
            gameObject.GetComponent<Waypoints>().goBack();
        }
    }
}
