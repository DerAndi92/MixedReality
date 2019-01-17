using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class EventFireCar : MonoBehaviour {

    private GameObject fireCar;
    private GameObject waterThrowerGameObject;
    private GameObject startWayPoint;
    private GameObject endWayPoint;
    private GameObject beforeEndWayPoint;

    private ObjectActionHandler objectActionHandlerScript;

    private ParticleSystem waterThrowerParticleSystem;

    private AudioSource sirene;

    private Waypoints waypointsFireCar;
    private bool isSireneOn = false;
    private bool isFireCarAtFireClearingPosition = false;
    private bool isWaterThrower = false;
    private bool isFireCarPrepared = false;
    private float timer;
    void Start()
    {
        fireCar = GameObject.Find("fireCar");
        sirene = GameObject.Find("FireCarSireneAudioSource").GetComponent<AudioSource>();
        startWayPoint = GameObject.Find("wayPoint_Parkplatz_Start");
        endWayPoint = GameObject.Find("wayPoint_Parkplatz_Ende");
        beforeEndWayPoint = GameObject.Find("wayPoint_Parkplatz_Einfahrt");
       
        waypointsFireCar = fireCar.GetComponent<Waypoints>();
        waterThrowerGameObject = GameObject.Find("Water_Thrower");
        waterThrowerParticleSystem = GameObject.Find("Water_Thrower").GetComponent<ParticleSystem>();
        timer = 0;
        fireCar.SetActive(false);
    }

    private void Update()
    {

        if(GameController.Instance.eventFireCar)
        {
            if(!isFireCarPrepared) {
                //Debug.Log("_______Prepare FireCar");

                GameController.Instance.isTrafficStopped = true;
                GameController.Instance.eventTrafficLightsInactive = true;
                Invoke("PrepareFireCar", 2);
                isFireCarPrepared = true;
            } 
            else if (GameController.Instance.eventFire && !GameController.Instance.isFireCleared)
            {
                if(!isSireneOn)
                {
                    sirene.Play();
                    isSireneOn = true;
                }
                // solange das Feuerwehrauto noch nicht vor Ort ist
                if (Vector3.Distance(fireCar.transform.position, beforeEndWayPoint.transform.position) < 1 && !GameController.Instance.isFireCleared)
                {
                    waypointsFireCar.current = 1;
                    timer = 0;
                } 
                else if (!isFireCarAtFireClearingPosition)
                {
                    if (GameController.Instance.fireCarStopAtWaypoint != null)
                    {
                        if (Vector3.Distance(fireCar.transform.position, GameController.Instance.fireCarStopAtWaypoint.transform.position) < 0.5)
                        {
                            isFireCarAtFireClearingPosition = true;
                            waypointsFireCar.isMoving = false;
                        }
                    }
                }
                else {
                    // Wasserwerfer starten
                    if (!isWaterThrower)
                    {
                        ActivateWaterThrower();
                    }
                    else
                    {
                        TimerUpdate();
                        if (timer > 3)
                        {
                            DeactivateWaterThrower();
                        }
                    }
                }
                
            }
            else
            {
                if (Vector3.Distance(fireCar.transform.position, endWayPoint.transform.position) < 1)
                {
                    ResetFireCar();
                }
            }
        }
    }

    void TimerUpdate()
    {
        timer += Time.deltaTime;
    }

    void ActivateWaterThrower()
    {
        if(GameController.Instance.fireCarStopAtWaypoint.name == "wayPoint_AutoRepair")
        {
            waterThrowerGameObject.transform.rotation = Quaternion.Euler(0, 0, 47);
        } else if (GameController.Instance.isWaterThrowerRight)
        {
            waterThrowerGameObject.transform.rotation = Quaternion.Euler(0, 90, 47);
        }
        else
        {
            waterThrowerGameObject.transform.rotation = Quaternion.Euler(0, -90, 47);
        }
        waterThrowerGameObject.transform.position = fireCar.transform.position;
        waterThrowerParticleSystem.Play();
        isWaterThrower = true;
        timer = 0;
    }

    void DeactivateWaterThrower()
    {
        sirene.Stop();
        waterThrowerParticleSystem.Stop();
        waypointsFireCar.isMoving = true;
        GameController.Instance.isFireCleared = true;
    
    }

    void PrepareFireCar()
    {
        fireCar.transform.position = startWayPoint.transform.position;
        timer = 0;
        fireCar.SetActive(true);
        fireCar.transform.eulerAngles = new Vector3(0, -90, 0);
        waypointsFireCar.current = 0;
        waypointsFireCar.isMoving = true;
        Invoke("StopTrafficStopper", 3);
    }

    void ResetFireCar()
    {
        waypointsFireCar.current = 0;
        timer = 0;

        GameController.Instance.eventFireCar = false;
        GameController.Instance.eventFire = false;
        GameController.Instance.isFireCleared = false;
        GameController.Instance.eventTrafficLightsInactive = false;
        isFireCarAtFireClearingPosition = false;
        isWaterThrower = false;
        fireCar.SetActive(false);
        isSireneOn = false;
        isFireCarPrepared = false;
    }

    void StopTrafficStopper()
    {
        GameController.Instance.isTrafficStopped = false;
    }

}

