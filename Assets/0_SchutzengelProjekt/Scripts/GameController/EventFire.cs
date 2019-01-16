﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EventFire : MonoBehaviour {

    private GameObject[] objects;
    private GameObject fireGameObject;
    private GameObject actualObject;
    private GameObject actualObjectBuilding;
    private GameObject actualObjectBurned;
    private GameObject actualObjectFirePosition;
    private ParticleSystem fireParticleSystem;
    private ObjectActionHandler objectActionHandlerScript;


    private float timer;
    private float fireDurationInSeconds = 10;
    private int objectNumber = 0;

    private bool isBuildingDisabled = false;
    private bool isFireInitialized = false;

    private AudioSource fireAudio;

    void Start () {
        fireAudio = GameObject.Find("FireAudioSource").GetComponent<AudioSource>();
        timer = fireDurationInSeconds;
        fireGameObject = GameObject.Find ("Fire");
        fireParticleSystem = GameObject.Find ("Fire").GetComponent<ParticleSystem> ();
        objects = GameObject.FindGameObjectsWithTag ("Part of Action");

    }

    void Update () {
        // wenn das Event Feuer ausgelöt wird
        if (GameController.Instance.eventFire && !GameController.Instance.eventFireDone) {
            // solange das Feuerwehrauto noch nicht getracked wurde
            if (!GameController.Instance.eventFireCar) {
                if(objectNumber < objects.Length) {
                    // alle x Sekunden geht das Feuer zum nähsten Gebäde
                    if (timer < fireDurationInSeconds) {
                        TimerUpdate();
                        // Das Gebäde wird eine Sekunde vor Feuerwechsel ausgelendet
                        if(timer > (fireDurationInSeconds - 1) && !isBuildingDisabled) {
                            DisableBuilding();
                            isBuildingDisabled = true;
                        }
                    } else {
                        TimerReset();
                    }
                    // Wenn alle Häser abgebrannt sind
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
                if(!isFireInitialized)
                {
                    TimerReset();
                }
                if (GameController.Instance.isFireCleared) {
                    StopFire();
                    timer = fireDurationInSeconds;
                    isFireInitialized = false;
                }
            }

        }
    }

    void TimerUpdate () {
        timer += Time.deltaTime;
    }

    void TimerReset () {
        actualObject = objects[objectNumber];
        objectActionHandlerScript = actualObject.GetComponent<ObjectActionHandler> ();
        GameController.Instance.fireCarStopAtWaypoint = objectActionHandlerScript.fireCarStopAtWaypoint;
        GameController.Instance.isWaterThrowerRight = objectActionHandlerScript.isWaterThrowerRight;
        actualObjectBuilding = GameObject.Find (actualObject.name + "/Building");
        actualObjectBurned = GameObject.Find (actualObject.name + "/Burned");
        actualObjectFirePosition = GameObject.Find (actualObject.name + "/FirePosition");
        isBuildingDisabled = false;
        isFireInitialized = true;
        StartFire();
        timer = 0;
        objectNumber++;
    }

    void StartFire () {
        if (objects.Length >= objectNumber) {
            if (actualObject != null) {
                fireAudio.Stop();
                fireAudio.Play();
                fireGameObject.transform.position = actualObjectFirePosition.transform.position;
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
        Debug.Log("Function DiasbleBuidling");
        actualObjectBuilding.SetActive (false);
        actualObjectBurned.transform.localScale = new Vector3 (1, 1, 1);
    }

}
