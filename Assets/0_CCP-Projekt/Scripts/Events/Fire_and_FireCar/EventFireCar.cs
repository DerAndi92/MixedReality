using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class EventFireCar : MonoBehaviour {

    private GameObject fireCar;
    private GameObject waterThrower;
    private GameObject carparkEntrance;
    
    private ParticleSystem waterThrowerParticleSystem;

    private AudioSource sirene;

    private Waypoints waypointsFireCar;

    private bool isSireneOn = false;
    private bool isFireCarAtFireClearingPosition = false;
    private bool isWaterThrower = false;
    private bool isFireCarPrepared = false;
    private bool isFireCarStarted = false;
    
    void Start()
    {
        fireCar = GameObject.Find("fireCar");
        sirene = GameObject.Find("FireCarSireneAudioSource").GetComponent<AudioSource>();
        carparkEntrance = GameObject.Find("wayPoint_Parkplatz_Einfahrt");
        waterThrower = GameObject.Find("Water_Thrower");
        waterThrowerParticleSystem = GameObject.Find("Water_Thrower").GetComponent<ParticleSystem>();
        waypointsFireCar = fireCar.GetComponent<Waypoints>();
        fireCar.SetActive(false);
    }

    private void Update()
    {
        if(GameController.Instance.eventFireCar)
        {
            if(!isFireCarPrepared) {
                // Stoppen der Autos vor der Ausfahrt, damit das Feuerwehrauto nicht kollidiert
                GameController.Instance.isTrafficStopped = true;
                // Deaktivieren der Ampeln, damit Feuerwehrauto schneller ans Ziel gelangt
                GameController.Instance.eventTrafficLightsInactive = true;
                Invoke("PrepareFireCar", 3);
                isFireCarPrepared = true;
                fireCar.SetActive(true);
            } 
            else if (GameController.Instance.eventFire && !GameController.Instance.isFireCleared)
            {
                if(!isSireneOn)
                {
                    sirene.Play();
                    isSireneOn = true;
                }
                // falls das Feuerwehrauto unterwegs ist und das Feuer währenddessen ausgelöst wird,
                // dann soll das Feuerwehrauto bei der Einfahrt weiterfahren zum Löschen des Feuers
                if (Vector3.Distance(fireCar.transform.position, carparkEntrance.transform.position) < 1 && !GameController.Instance.isFireCleared)
                {
                    waypointsFireCar.current = 1;
                } 
                // solange das Feuerwehrauto noch nicht beim Feuer ist
                else if (!isFireCarAtFireClearingPosition)
                {
                    if (GameController.Instance.fireCarStopAtWaypoint != null)
                    {
                        if (Vector3.Distance(fireCar.transform.position, GameController.Instance.fireCarStopAtWaypoint.transform.position) < 0.5)
                        {
                            isFireCarAtFireClearingPosition = true;
                            // isMoving auf false, damit die Feuerwehr stehen bleibt
                            waypointsFireCar.isMoving = false;
                        }
                    }
                }
                else {
                    // Wasserwerfer starten
                    if (!isWaterThrower)
                    {
                        ActivateWaterThrower();
                        Invoke("DeactivateWaterThrower", 3);
                    }
                }
                
            }
            else
            {
                if (!waypointsFireCar.isMoving && isFireCarStarted)
                {
                    ResetFireCar();
                }
            }
        }

    }


    void ActivateWaterThrower()
    {
        // Bestimmung der Rotation des Wasserstrahls - links oder rechts vom Feuerwehrauto
        // Sonderfall: "wayPoint_AutoRepair", da andere Straßenrotation
        if(GameController.Instance.fireCarStopAtWaypoint.name == "wayPoint_AutoRepair")
        {
            waterThrower.transform.rotation = Quaternion.Euler(0, 0, 47);
        } 
        else if (GameController.Instance.isWaterThrowerRight)
        {
            waterThrower.transform.rotation = Quaternion.Euler(0, 90, 47);
        }
        else
        {
            waterThrower.transform.rotation = Quaternion.Euler(0, -90, 47);
        }
        waterThrower.transform.position = fireCar.transform.position;
        waterThrowerParticleSystem.Play();
        isWaterThrower = true;
    }

    void DeactivateWaterThrower()
    {
        sirene.Stop();
        waterThrowerParticleSystem.Stop();
        // Feuerwehrauto weiterfahren lassen
        waypointsFireCar.isMoving = true;
        GameController.Instance.isFireCleared = true;
    }

    void PrepareFireCar()
    {
        if(GameController.Instance.eventFire && !GameController.Instance.isFireCleared){
            sirene.Play();
            isSireneOn = true;
        }
        isFireCarStarted = true;
        waypointsFireCar.current = 0;
        waypointsFireCar.isMoving = true;
        Invoke("StopTrafficStopper", 3);
    }

    void ResetFireCar()
    {
        waypointsFireCar.current = 0;
        GameController.Instance.eventFireCar = false;
        GameController.Instance.eventFire = false;
        GameController.Instance.isFireCleared = false;
        GameController.Instance.eventTrafficLightsInactive = false;
        isFireCarAtFireClearingPosition = false;
        isWaterThrower = false;
        fireCar.SetActive(false);
        isSireneOn = false;
        isFireCarPrepared = false;
        isFireCarStarted = false;
    }

    void StopTrafficStopper()
    {
        GameController.Instance.isTrafficStopped = false;
    }

}

