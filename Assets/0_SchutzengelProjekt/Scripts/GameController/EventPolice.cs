﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPolice : MonoBehaviour
{

    public GameObject[] policeMans;

    private GameObject policeCar;
    private GameObject startWayPoint;
    private GameObject endWayPoint; 

    private AudioSource sirene;

    private Waypoints waypointsCar;
    private Waypoints waypointsLong;
    private Waypoints waypointsShort;
    private Waypoints waypointsShort_Back;

    private bool isCarPrepared = false;
    private bool isAtMall = false;
    private bool goBack = false;

    // Start is called before the first frame update
    void Start()
    {

        // Deactivate PoliceMans
        foreach (GameObject p in policeMans)
        {
            p.SetActive(false);
        }

        policeCar = GameObject.Find("bomb_Police");
        sirene = GameObject.Find("Police_Sirene").GetComponent<AudioSource>();
        startWayPoint = GameObject.Find("wayPoint_Parkplatz_Start");
        endWayPoint = GameObject.Find("wayPoint_Parkplatz_Ende");

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
                if (Vector3.Distance(policeCar.transform.position, endWayPoint.transform.position) < 2)
                {
                    ResetCar();
                }
            }
        }
    }

    void PrepareCar()
    {
        policeCar.transform.position = startWayPoint.transform.position;
        policeCar.transform.eulerAngles = new Vector3(0, -90, 0);
        waypointsCar.current = 0;
        policeCar.SetActive(true);

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