using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrafficLight : MonoBehaviour {

    public float checkInterval = 5;

    private bool redForCar = false;
    private float delta = 0;
    private List<Collider> cars = new List<Collider>();
    private List<Collider> people = new List<Collider>();

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        delta += Time.deltaTime;

        if (delta >= checkInterval)
        {
            delta = 0;
            check();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Waypoints wp = other.GetComponent<Waypoints>();
        if (wp != null)
        {
            
            if(redForCar && wp.collisionGroup != "people")
            {
                wp.isMoving = false;
                cars.Add(other);
            }
            else if (!redForCar && wp.collisionGroup == "people")
            {
                wp.isMoving = false;
                other.GetComponent<Moveit>().stop();
                people.Add(other);
            }
            else if(redForCar && wp.collisionGroup == "people")
            {
                people.Add(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (redForCar && people.Contains(other))
        {
            people.Remove(other);
        }
    }

    private void check()
    {
        if(redForCar && people.Count() == 0)
        {
            redForCar = false;
            foreach(Collider c in cars)
            {
                c.GetComponent<Waypoints>().isMoving = true;
            }
            cars.Clear();
        }
        else if(!redForCar && people.Count() > 0)
        {
            redForCar = true;
            foreach (Collider p in people)
            {
                p.GetComponent<Waypoints>().isMoving = true;
                p.GetComponent<Moveit>().start();
            }
        }

    }
    
}
