using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class EventFireCar : MonoBehaviour {

    private GameObject fireCar;
    private GameObject waterThrowerGameObject;
    private GameObject startWayPoint;
    private GameObject endWayPoint;

    private ObjectActionHandler objectActionHandlerScript;

    private ParticleSystem waterThrowerParticleSystem;

    private AudioSource sirene;

    private Waypoints waypointsFireCar;
    private bool isSireneOn = false;
    private bool isFireCarAtPosition = false;
    private bool isWaterThrower = false;
    private bool isFireCarPrepared = false;
    private float timer;
    void Start()
    {
        fireCar = GameObject.Find("fireCar");
        sirene = GameObject.Find("FireCarSireneAudioSource").GetComponent<AudioSource>();
        startWayPoint = GameObject.Find("wayPoint_Parkplatz_Start");
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
            if(!isFireCarPrepared) {
                Debug.Log("_______Prepare FireCar");
                PrepareFireCar();
            }
            if (GameController.Instance.eventFire && !GameController.Instance.isFireCleared)
            {
                if(!isSireneOn)
                {
                    sirene.Play();
                    isSireneOn = true;
                }
                // solange das Feuerwehrauto noch nicht vor Ort ist
                if (!isFireCarAtPosition)
                {
                    if (Vector3.Distance(fireCar.transform.position, GameController.Instance.fireCarStopAtWaypoint.transform.position) < 0.5)
                    {
                        Debug.Log("_______Position FireCar");
                        isFireCarAtPosition = true;
                        waypointsFireCar.isMoving = false;
                    }
                }
                else
                {
                    // Wasserwerfer starten
                    if (!isWaterThrower)
                    {
                        Debug.Log("_______Water FireCar");
                        ActivateWaterThrower();
                    }
                    else
                    {
                        TimerUpdate();
                        if (timer > 3)
                        {
                            Debug.Log("_______DeWater FireCar");
                            DeactivateWaterThrower();
                        }
                    }
                }
                
            }
            else
            {
                if (Vector3.Distance(fireCar.transform.position, endWayPoint.transform.position) < 1)
                {
                    Debug.Log("_______Reset FireCar");
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
        fireCar.transform.eulerAngles = new Vector3(0, -90, 0);
        fireCar.SetActive(true);
        waypointsFireCar.current = 0;
        isFireCarPrepared = true;
        waypointsFireCar.isMoving = true;
    }

    void ResetFireCar()
    {
        GameController.Instance.eventFireCar = false;
        GameController.Instance.eventFire = false;
        GameController.Instance.isFireCleared = false;
        isFireCarAtPosition = false;
        isWaterThrower = false;
        fireCar.SetActive(false);
        waypointsFireCar.current = 0;
        isFireCarPrepared = false;
        timer = 0;
        isSireneOn = false;
    }

}

