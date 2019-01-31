using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPolice : MonoBehaviour
{

    public GameObject[] policeMans;
    private GameObject policeCar;

    private AudioSource sirene;

    private Waypoints waypointsCar;
    private Waypoints waypointsLong;
    private Waypoints waypointsShort;
    private Waypoints waypointsShort_Back;

    private bool isCarPrepared = false;
    private bool isAtMall = false;
    private bool goBack = false;
    private bool isCarStarted = false;

    void Start()
    {
        // Deactivate PoliceMans
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

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.eventPolice)
        {

            if(!isCarPrepared)
            {
                GameController.Instance.isTrafficStopped = true;
                Invoke("PrepareCar", 3);
                isCarPrepared = true;
                policeCar.SetActive(true);

            }
            else if (!isAtMall && isCarPrepared && GameController.Instance.eventPoliceAtMall)
            {
                isAtMall = true;
                if (!GameController.Instance.eventBombDone)
                {
                    // Verbrecher laufen los
                    foreach (GameObject p in policeMans)
                    {
                        p.SetActive(true);
                        p.GetComponent<Moveit>().start();
                        p.GetComponent<Waypoints>().isMoving = true;
                    }
                } else
                {
                    GameController.Instance.eventBombRemoved = 4;
                }
                
            }
            else if(!goBack && isAtMall && GameController.Instance.eventBombRemoved == 4)
            {
                GameController.Instance.isTrafficStopped = true;
                sirene.Stop();
                Invoke("CarDriveBack", 4);
                goBack = true;
            }
            else
            {
                if (!waypointsCar.isMoving && isCarStarted)
                {
                    ResetCar();
                }
            }
        }
       
    }

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

    void StopTrafficStopper()
    {
        GameController.Instance.isTrafficStopped = false;
    }

    void ResetCar()
    {
        policeCar.SetActive(false);
        waypointsCar.current = 0;
        isCarStarted = false;
        waypointsCar.isMoving = false;
        isCarPrepared = false;
        GameController.Instance.eventPolice = false;
    }

    void CarDriveBack()
    {
        waypointsCar.changeWaypoints(waypointsShort_Back.waypoints, waypointsShort_Back.rotations, waypointsCar.speed, true, false, false, false);
        Invoke("StopTrafficStopper", 3);
    }
}
