using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScaryAction : MonoBehaviour
{

    private bool done = false; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
