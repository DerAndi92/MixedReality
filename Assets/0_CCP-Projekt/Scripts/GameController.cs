﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public bool isCityTracked = false;
    public bool isEventInPlace = false;
    public bool isRescueInPlace = false;

    public bool isTrafficStopped = false;
    public bool eventTrafficLightsInactive = false;

    public bool isFireTargetTracked = false;
    public bool isUfoTargetTracked = false;
    public bool isFireCarTargetTracked = false;
    public bool isHelicopterTargetTracked = false;
    public bool isTornadoTargetTracked = false;
    public bool isBombTargetTracked = false;
    public bool isPoliceTargetTracked = false;

    // Event Fire & FireCar
    public bool eventFireDone = false;
    public bool eventFire = false;
    public bool eventFireCar = false;
    public bool isFireCleared = false;
    public bool isWaterThrowerRight = false;
    public GameObject fireCarStopAtWaypoint;

    // Event Ufo & Heli
    public bool eventUfoDone = false;
    public bool eventUfo = false;
    public bool eventHelicopter = false;
    public bool ufoIsShot = false;

    // Event Tornado
    public bool eventTornado = false;
    public bool eventTornadoDone = false;

    // Event Bomb & Police
    public bool eventBomb = false;
    public short eventBombPlaced = 0;
    public bool eventPolice = false;
    public bool eventPoliceAtMall = false;
    public short eventBombRemoved = 0;
    public bool eventBombDone = false;

    // Event Reset
    public bool eventReset = false;
    public int waitToNextReset = 5;
    public Int32 resetTime = 0;

    public bool finalExplosion = false;

    private static GameController instance;

    public static GameController Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<GameController>();
            }
            return instance;
        }
    }

   	void Start () {
        fireCarStopAtWaypoint = null;
        DontDestroyOnLoad(gameObject);
	}

}
