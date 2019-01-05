using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class EventFireCar : MonoBehaviour {

    private GameObject fireCar;
    private GameObject waterThrowerGameObject;
    private GameObject endWayPoint;
    private ObjectActionHandler objectActionHandlerScript;

    private ParticleSystem waterThrowerParticleSystem;

    private AudioSource sirene;

    private Waypoints waypointsFireCar;
    private bool isFireCarAtPosition = false;
    private bool isWaterThrower = false;
    private bool isFireCarPrepared = false;
    private float timer;
    void Start()
    {
        fireCar = GameObject.Find("fireCar");
        sirene = GameObject.Find("FireCarSireneAudioSource").GetComponent<AudioSource>();
        endWayPoint = GameObject.Find("wayPoint_Parkplatz_Ende");
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
            fireCar.SetActive(true);
            if (GameController.Instance.eventFire && !GameController.Instance.isFireCleared)
            {
                if (!GameController.Instance.fireCarSend)
                {
                    GameController.Instance.fireCarSend = true;
                    waypointsFireCar.isMoving = true;
                    sirene.Play();
                }
                else
                {
                    // solange das Feuerwehrauto noch nicht vor Ort ist
                    if (!isFireCarAtPosition)
                    {
                        if (Vector3.Distance(fireCar.transform.position, GameController.Instance.fireCarStopAtWaypoint.transform.position) < 0.5)
                        {
                            isFireCarAtPosition = true;
                            waypointsFireCar.isMoving = false;
                        }
                    }
                    else
                    {
                        // Wasserwerfer starten
                        if (!isWaterThrower)
                        {
                            Debug.Log("GameController.Instance.isWaterThrowerRight " + GameController.Instance.isWaterThrowerRight);

                            if (GameController.Instance.isWaterThrowerRight)
                            {
                                waterThrowerGameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            }
                            else
                            {
                                waterThrowerGameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
                            }
                            waterThrowerGameObject.transform.position = fireCar.transform.position;
                            waterThrowerParticleSystem.Play();
                            isWaterThrower = true;
                            timer = 0;
                        }
                        else
                        {
                            // Wasserwerfer 3 Sekunden laufen lassen, dann Feuerwehrauto weiter
                            timerUpdate();
                            if (timer > 3)
                            {
                                sirene.Stop();
                                waterThrowerParticleSystem.Stop();
                                waypointsFireCar.isMoving = true;
                                GameController.Instance.isFireCleared = true;
                            }

                        }
                    }
                }
            }
            else
            {
                if (Vector3.Distance(fireCar.transform.position, endWayPoint.transform.position) < 0.5)
                {
                    GameController.Instance.eventFireCar = false;
                    GameController.Instance.eventFire = false;
                }
            }
        }
        else
        {
            if(!isFireCarPrepared) { 
                waypointsFireCar.current = 0;
                waypointsFireCar.isMoving = true;
                GameController.Instance.fireCareOnEvent = true;
                isFireCarPrepared = true;
            }
            else
            {
                if (Vector3.Distance(fireCar.transform.position, endWayPoint.transform.position) < 0.5)
                {
                    GameController.Instance.eventFireCar = false;
                }
            }
        }
    }
    void timerUpdate()
    {
        timer += Time.deltaTime;
    }


}

