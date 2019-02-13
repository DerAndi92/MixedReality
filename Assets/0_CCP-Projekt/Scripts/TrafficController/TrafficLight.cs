using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrafficLight : MonoBehaviour {

    // Wie viele Sekunden sollen vergehen, bis die Ampelschaltung geprüft wird?
    public float checkInterval = 3;

    private bool redForCar = false;
    private float delta = 0;
    private List<Collider> cars = new List<Collider>();
    private List<Collider> people = new List<Collider>();

    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        delta += Time.deltaTime;

        // Ampelschaltung wird nach X Sekunden überprüft
        if (delta >= checkInterval)
        {
            delta = 0;
            check();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        // An Hand der Waypoints wird überprüft, um welches Objekt es sich handelt.

        Waypoints wp = other.GetComponent<Waypoints>();
        if (wp != null)
        {

            // Wenn die Ampel rot für Autos ist ist und ein Auto an der Ampel ankommt, stoppt dieses und wird im Cars Array aufgenommen.
            if (redForCar && wp.collisionGroup != "people")
            {
                wp.isMoving = false;
                cars.Add(other);
            }

            // Wenn die Ampel rot für Leute ist und sie an der Ampel ankommen, stopppen sie und werden in das people Array aufgenommen.
            else if (!redForCar && wp.collisionGroup == "people")
            {
                wp.isMoving = false;
                other.GetComponent<Moveit>().stop();
                people.Add(other);
            }

            // Auch wenn die Ampel für Leute grün ist und sie über die Straße gehen können, werden sie in das people Array aufgenommen.
            else if(redForCar && wp.collisionGroup == "people")
            {
                people.Add(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Wenn die Ampel grün für Leute ist und ein Mensch die Ampel verlässt (über Straße gegangen) wurd er aus dem people Array entfernt.
        if (redForCar && people.Contains(other))
        {
            people.Remove(other);
        }
    }

    // Die Ampelschaltung wird überprüft
    private void check()
    {
        // Wenn aktuell rot für Autos ist, aber keine Menschen mehr über die Straße gehen (people array leer), wird die Ampel umgeschaltet.
        // Die Autos, die gestopppt wurden (im cars array), fahren wieder los und werden aus dem array entfernt.
        if (redForCar && people.Count() == 0)
        {
            redForCar = false;
            foreach (Collider c in cars)
            {
                c.GetComponent<Waypoints>().isMoving = true;
            }
            cars.Clear();
        }

        // Wenn grün für die Autos ist, aber Leute an der Ampel warten, wird die Ampel umgeschaltet. 
        // Alle Leute im people Array fangen dann wieder an zu laufen
        else if(!redForCar && people.Count() > 0)
        {
            if(!GameController.Instance.eventTrafficLightsInactive) { //Wenn bestimmte Events aktiv sind, sollen keine Ampeln für Autos auf rot gehen! Damit diese freie Fahrt haben. (Feuerwehr z.B.)
                redForCar = true;
                foreach (Collider p in people)
                {
                    p.GetComponent<Waypoints>().isMoving = true;
                    p.GetComponent<Moveit>().start();
                }
            }
        }

    }
    
}
