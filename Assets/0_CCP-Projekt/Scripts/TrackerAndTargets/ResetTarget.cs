﻿using UnityEngine;
using UnityEngine.Playables;
using System;

using System.Collections;
using Vuforia;
using UnityEngine.SceneManagement;

public class ResetTarget : MonoBehaviour, ITrackableEventHandler
{
    private TrackableBehaviour mTrackableBehaviour;
    private AudioSource destructionAudioSource;
    private GameObject explosion;
    private GameObject city;
    private Light cityLight;

    private bool lightSwitch = true;
    private bool isExploding = false;
    private bool isExploded = false;
    private bool timeLeft = true;

    void Start()
    {
        FindGameObjects();

        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    void Update()
    {
        Int32 timeNow = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        if(GameController.Instance.resetTime > 0)  {
            timeLeft = (timeNow - GameController.Instance.resetTime) > 10;
        }
        // Aktiv wenn die Stadt noch nicht explodiert ist
        if(!isExploded) { 
            // Roter Alarm wenn aktiv
            if(!isExploding && GameController.Instance.eventReset)
            {

                cityLight.color += (Color.red / 2.5f) * Time.deltaTime;

                if (lightSwitch)
                {
                    cityLight.intensity -= Time.deltaTime * 0.8f;
                    if (cityLight.intensity <= 0.1) lightSwitch = false;
                }
                else
                {
                    cityLight.intensity += Time.deltaTime * 0.8f;
                    if (cityLight.intensity >= 1) lightSwitch = true;
                }
            }

            // Bei Explosion die Stadt zerstören
            if (isExploding && GameController.Instance.eventReset)
            {
                cityLight.color += (Color.white / 2f) * Time.deltaTime;
                cityLight.intensity += Time.deltaTime * 13f;
                if (cityLight.intensity >= 6)
                {
                    city.SetActive(false);
                    isExploding = false;
                    isExploded = true;
                }
            }
        }
    }

    void FindGameObjects()
    {
        destructionAudioSource = GameObject.Find("DestructionAudioSource").GetComponent<AudioSource>();
        explosion = GameObject.Find("FinalExplosion");
        city = GameObject.Find("City");
        explosion.SetActive(false);
        cityLight = GameObject.Find("Directional Light").GetComponent<Light>();
    }

    void StartReset() {
        explosion.SetActive(true);
        isExploding = true;
        Invoke("DoReset", 3);

    }

    void DoReset()
    {
        GameController.Instance.isEventInPlace = false;
        GameController.Instance.isRescueInPlace = false;

        GameController.Instance.eventTrafficLightsInactive = false;
        GameController.Instance.isTrafficStopped = false;

        GameController.Instance.isFireTargetTracked = false;
        GameController.Instance.isUfoTargetTracked = false;
        GameController.Instance.isFireCarTargetTracked = false;
        GameController.Instance.isHelicopterTargetTracked = false;
        GameController.Instance.isTornadoTargetTracked = false;
        GameController.Instance.isBombTargetTracked = false;
        GameController.Instance.isPoliceTargetTracked = false;


        // Event Fire & FireCar
        GameController.Instance.eventFire = false;
        GameController.Instance.eventFireCar = false;
        GameController.Instance.isFireCleared = false;
        GameController.Instance.isWaterThrowerRight = false;
        GameController.Instance.eventFireDone = false;


        // Event Ufo & Heli
        GameController.Instance.eventUfoDone = false;
        GameController.Instance.eventUfo = false;
        GameController.Instance.eventHelicopter = false;
        GameController.Instance.ufoIsShot = false;

        // Event Tornado
        GameController.Instance.eventTornado = false;
        GameController.Instance.eventTornadoDone = false;

        // Event Bomb & Police
        GameController.Instance.eventBomb = false;
        GameController.Instance.eventBombPlaced = 0;
        GameController.Instance.eventPolice = false;
        GameController.Instance.eventPoliceAtMall = false;
        GameController.Instance.eventBombDone = false;

        GameController.Instance.resetTime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        SceneManager.LoadScene("CCP-City");
        GameController.Instance.eventReset = false;

    }
    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        // Ein Marker wird entdeckt
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED)
        {

            if (!GameController.Instance.eventReset && timeLeft)
            {
                GameController.Instance.eventReset = true;
                destructionAudioSource.Play();
                Invoke("StartReset", 6.4f);
            }
        }
    }
}