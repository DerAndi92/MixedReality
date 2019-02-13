using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPolice : MonoBehaviour
{

    public GameObject[] policeMans;
    private GameObject policeCar;

    private AudioSource sirene;

    private Waypoints waypointsCar;

    // Wegpunkte für die Fahrt in der Stadt
    private Waypoints waypointsLong;
    // Wegpunkte für die Fahrt zur Mall
    private Waypoints waypointsShort;
    // Wegpunkte für die Fahrt von Mall zurück zum Anfang
    private Waypoints waypointsShort_Back;

    private bool isCarPrepared = false;
    private bool isAtMall = false;
    private bool goBack = false;
    private bool isCarStarted = false;

    void Start()
    {
        // Alle Polizisten bei der Mall werden inaktiv geschaltet, damit sie nicht gesehen werden.
        foreach (GameObject p in policeMans)
        {
            p.SetActive(false);
        }

        policeCar = GameObject.Find("bomb_Police");
        sirene = GameObject.Find("Police_Sirene").GetComponent<AudioSource>();

        waypointsCar = policeCar.GetComponent<Waypoints>();
        waypointsLong = GameObject.Find("PoliceCarWPLong").GetComponent<Waypoints>();
        waypointsShort = GameObject.Find("PoliceCarWPShort").GetComponent<Waypoints>();
        waypointsShort_Back = GameObject.Find("PoliceCarWPShort_Back").GetComponent<Waypoints>();

        policeCar.SetActive(false);
    }

    void Update()
    {
        // ist das Polizei-Event ausgelöst?
        if (GameController.Instance.eventPolice)
        {

            // Wurde das Auto bereits für die Fahrt vorbereitet?
            if (!isCarPrepared)
            {
                GameController.Instance.isTrafficStopped = true;
                Invoke("PrepareCar", 3);
                isCarPrepared = true;
                policeCar.SetActive(true);

            }

            // Ist das Auto für die Fahrt vorbereitet aber noch nicht bei der Mall angekommen?
            else if (!isAtMall && isCarPrepared && GameController.Instance.eventPoliceAtMall)
            {
                isAtMall = true;

                // Wurden die Bomben bereits entschärft?
                if (!GameController.Instance.eventBombDone)
                {
                    // Polizisten laufen los, um Bomben zu entschärfen
                    foreach (GameObject p in policeMans)
                    {
                        p.SetActive(true);
                        p.GetComponent<Moveit>().start();
                        p.GetComponent<Waypoints>().isMoving = true;
                    }
                }
                else
                {
                    GameController.Instance.eventBombRemoved = 4;
                }

            }

            // Sind die Polizisten alle schon zurück gelaufen und kann das Polizei Auto zurückfahren?
            else if (!goBack && isAtMall && GameController.Instance.eventBombRemoved == 4)
            {
                GameController.Instance.isTrafficStopped = true;
                sirene.Stop();
                Invoke("CarDriveBack", 4);
                goBack = true;
            }

            // Das Auto ist wieder zurück in der Station und das Event kann resettet werden
            else
            {
                if (!waypointsCar.isMoving && isCarStarted)
                {
                    ResetCar();
                }
            }
        }
       
    }


    // Das Auto wird für die fahrt vorbereitet
    // Wenn das Bombenevent aktiv ist, fährt das Auto zur Mall. Wenn nicht, fährt es einmal durch die Stadt.
    // Entsprechend werden die Waypoints ausgetauscht.
    // Außerdem wird der Verkehr vor der Rettungsstation gestopppt, damit die Autos nicht zusammenstoßen.
    void PrepareCar()
    {
        waypointsCar.current = 0;
        isCarStarted = true;
        if (GameController.Instance.eventBombPlaced == 4 && !GameController.Instance.eventBombDone)
        {
            waypointsCar.changeWaypoints(waypointsShort.waypoints, waypointsShort.rotations, waypointsCar.speed, true, false, false, false);
            sirene.Play();
        } else
        {
            waypointsCar.changeWaypoints(waypointsLong.waypoints, waypointsLong.rotations, waypointsCar.speed, true, false, false, false);
        }
        

        Invoke("StopTrafficStopper", 3);
        
    }

    // Verkehr vor der Rettungsstation soll wieder weiterfahren
    void StopTrafficStopper()
    {
        GameController.Instance.isTrafficStopped = false;
    }

    // Nach dem Event wird das Auto wieder korrekt positioniert.
    void ResetCar()
    {
        policeCar.SetActive(false);
        waypointsCar.current = 0;
        isCarStarted = false;
        waypointsCar.isMoving = false;
        isCarPrepared = false;
        GameController.Instance.eventPolice = false;
    }

    // Die Polizei soll von der Mall zurück zur Rettungsstation fahren. Dafür werden die Waypoints entsprechend ausgetauscht.
    void CarDriveBack()
    {
        waypointsCar.changeWaypoints(waypointsShort_Back.waypoints, waypointsShort_Back.rotations, waypointsCar.speed, true, false, false, false);
        Invoke("StopTrafficStopper", 3);
    }
}
