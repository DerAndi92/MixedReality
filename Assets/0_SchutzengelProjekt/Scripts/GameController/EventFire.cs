using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EventFire : MonoBehaviour {

    private float fireDifferenceNumber = 10;
    private GameObject[] objects;
    private int objectNumber = 0;
    private GameObject fireGameObject;
    private ParticleSystem fireParticleSystem;
    private GameObject waterGameObject;
    private ParticleSystem waterParticleSystem;
    private GameObject fireCar;
    private Waypoints fireCarWayPointScript;
    private GameObject fireCarStopAtWaypoint;
    float timer;

    private GameObject actualObject;
    private GameObject actualObjectBuilding;
    private GameObject actualObjectBurned;
    private GameObject actualObjectFirePosition;
    private ObjectActionHandler objectActionHandlerScript;

    private bool isBuildingDisabled = false;
    private bool isWaterThrower = false;
    private bool isFireCarAtPosition = false;
    // Use this for initialization
    void Start () {
        // find objects
        fireGameObject = GameObject.Find ("Fire");
        fireParticleSystem = GameObject.Find ("Fire").GetComponent<ParticleSystem> ();
        waterGameObject = GameObject.Find ("Water_Thrower");
        waterParticleSystem = GameObject.Find ("Water_Thrower").GetComponent<ParticleSystem> ();

        fireCar = GameObject.Find ("fireCar");
        fireCarWayPointScript = fireCar.GetComponent<Waypoints> ();

        timer = fireDifferenceNumber;
        if (objects == null)
            objects = GameObject.FindGameObjectsWithTag ("Part of Action");
    }

    void Update () {
        // wenn das Event Feuer ausgelöst wird
        if (GameController.Instance.eventFire) {
            // solange das Feuerwehrauto noch nicht getracked wurde
            if (!GameController.Instance.trackedFireCarTarget) {

                // alle x Sekunden geht das Feuer zum nächsten Gebäude
                if (timer < fireDifferenceNumber) {
                    timerUpdate ();
                    if(timer > (fireDifferenceNumber - 2) && !isBuildingDisabled) {
                        disableBuilding ();
                        isBuildingDisabled = true;
                    }
                } else {
                    timerReset ();
                }
                // wenn das Feuerwehrauto getracked wird
            } else {
                // Feuerwehrauto wird losgeschickt
                if (!GameController.Instance.fireCarSend) {
                    GameController.Instance.fireCarSend = true;
                    fireCarWayPointScript.isMoving = true;
                    fireCarStopAtWaypoint = objectActionHandlerScript.fireCarStopAtWaypoint;
                } else {
                    // solange das Feuerwehrauto noch nicht vor Ort ist
                    if (!isFireCarAtPosition) {
                        if (Vector3.Distance (fireCar.transform.position, fireCarStopAtWaypoint.transform.position) < 0.5) {
                            isFireCarAtPosition = true;
                            fireCarWayPointScript.isMoving = false;
                        }
                    // Feuerwehrauto ist vor Ort
                    } else {
                        // Wasserwerfer starten
                        if (!isWaterThrower) {
                            if (objectActionHandlerScript.isWaterThrowerRight) {
                                waterGameObject.transform.rotation = Quaternion.Euler (0, 90, 0);
                            } else {
                                waterGameObject.transform.rotation = Quaternion.Euler (0, -90, 0);
                            }
                            waterGameObject.transform.position = fireCar.transform.position;
                            waterParticleSystem.Play ();
                            isWaterThrower = true;
                            timer = 0;
                        // Wasserwerfer 3 Sekunden laufen lassen, dann Feuerwehrauto weiter
                        } else {
                            timerUpdate ();
                            if (timer > 3) {
                                waterParticleSystem.Stop ();
                                fireParticleSystem.Stop();
                                fireCarWayPointScript.isMoving = true;
                            }

                        }
                    }
                }

            }
        }
    }

    void timerUpdate () {
        timer += Time.deltaTime;
    }

    void timerReset () {
        actualObject = objects[objectNumber];
        Debug.Log (actualObject.name);
        objectActionHandlerScript = actualObject.GetComponent<ObjectActionHandler> ();
        actualObjectBuilding = GameObject.Find (actualObject.name + "/Building");
        actualObjectBurned = GameObject.Find (actualObject.name + "/Burned");
        actualObjectFirePosition = GameObject.Find (actualObject.name + "/FirePosition");
         isBuildingDisabled = false;
        startFire ();
        timer = 0;
        objectNumber++;
    }

    void startFire () {
        if (objects.Length >= objectNumber) {
            if (actualObject != null) {
                Debug.Log (actualObjectFirePosition.transform.position);
                fireParticleSystem.Play ();
                fireGameObject.transform.position = actualObjectFirePosition.transform.position;
            }
        }
    }

    void stopFireEvent () {
        waterParticleSystem.Stop ();
    }

    void disableBuilding () {
        actualObjectBuilding.SetActive (false);
        actualObjectBurned.transform.localScale = new Vector3 (1, 1, 1);
    }

}