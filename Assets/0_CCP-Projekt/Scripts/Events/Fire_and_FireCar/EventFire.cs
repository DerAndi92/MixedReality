using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EventFire : MonoBehaviour {

    private GameObject[] houseObjects;
    private GameObject fireGameObject;
    private GameObject actualHouse;
    private GameObject actualHouseBuilding;
    private GameObject actualHouseBurned;
    private GameObject actualHouseFirePosition;
    private ParticleSystem fireParticleSystem;
    private FireActionHandler fireActionHandlerScript;

    private float timer;
    private float fireDurationInSeconds = 10;
    private int houseNumber = 0;

    private bool isBuildingDisabled = false;
    private bool isFireInitialized = false;

    private AudioSource fireAudio;

    void Start () {
        FindGameObjects();
        timer = fireDurationInSeconds;
    }

    void Update () {
        // wenn das Event Feuer ausgelöst wird und das Feuerevent noch nicht durchgeführt wurde
        if (GameController.Instance.eventFire && !GameController.Instance.eventFireDone) {
            if (!GameController.Instance.eventFireCar) {
                if(houseNumber < houseObjects.Length) {
                    // alle x Sekunden geht das Feuer zum nähsten Gebäde
                    if (timer < fireDurationInSeconds) {
                        TimerUpdate();
                        // Das Gebäde wird eine Sekunde vor Feuerwechsel ausgeblendet
                        if(timer > (fireDurationInSeconds - 1) && !isBuildingDisabled) {
                            DisableBuilding();
                            isBuildingDisabled = true;
                        }
                    } else {
                        HandleFire();
                    }
                // Wenn alle Häuser abgebrannt sind
                } else {
                    if (timer < fireDurationInSeconds)
                    {
                        TimerUpdate();
                    } else {
                        DisableBuilding();
                        StopFire();
                    }
                }
            // wenn das Feuerwehrauto getracked wird
            } else {
                // falls das Feuerwehrauto bereits vor der Aktivierung des Events losgeschickt wurde 
                // und noch unterwegs ist, soll das Feuer trotzdem aktiviert werden
                if(!isFireInitialized)
                {
                    HandleFire();
                }
                if (GameController.Instance.isFireCleared) {
                    StopFire();
                    timer = fireDurationInSeconds;
                    isFireInitialized = false;
                }
            }

        }
    }

    void FindGameObjects()
    {
        fireAudio = GameObject.Find("FireAudioSource").GetComponent<AudioSource>();
        fireGameObject = GameObject.Find("Fire");
        fireParticleSystem = GameObject.Find("Fire").GetComponent<ParticleSystem>();
        houseObjects = GameObject.FindGameObjectsWithTag("Part of Action");
    }

    void TimerUpdate () {
        timer += Time.deltaTime;
    }

    void HandleFire () {
        actualHouse = houseObjects[houseNumber];
        fireActionHandlerScript = actualHouse.GetComponent<FireActionHandler> ();

        //WayPoint und Wasserwerferposition an GameController -> wird vom EventFireCar ausgelesen
        GameController.Instance.fireCarStopAtWaypoint = fireActionHandlerScript.fireCarStopAtWaypoint;
        GameController.Instance.isWaterThrowerRight = fireActionHandlerScript.isWaterThrowerRight;

        // Objekte finden und setzen für den neuen Brand
        actualHouseBuilding = GameObject.Find (actualHouse.name + "/Building");
        actualHouseBurned = GameObject.Find (actualHouse.name + "/Burned");
        actualHouseFirePosition = GameObject.Find (actualHouse.name + "/FirePosition");

        isBuildingDisabled = false;
        isFireInitialized = true;
        StartFire();
        timer = 0;
        houseNumber++;
    }

    void StartFire () {
        if (houseObjects.Length >= houseNumber) {
            if (actualHouse != null) {
                fireAudio.Stop();
                fireAudio.Play();
                fireGameObject.transform.position = actualHouseFirePosition.transform.position;
                fireParticleSystem.Play();
            }
        }
    }

    void StopFire()
    {
        fireParticleSystem.Stop();
        fireAudio.Stop();
        GameController.Instance.eventFire = false;
        GameController.Instance.eventFireDone = true;
    }

    void DisableBuilding () {
        actualHouseBuilding.SetActive (false);
        actualHouseBurned.transform.localScale = new Vector3 (1, 1, 1);
    }

}
