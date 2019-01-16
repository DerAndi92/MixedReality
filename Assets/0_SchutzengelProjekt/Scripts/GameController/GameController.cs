using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public List<MonoBehaviour> eventSubscribedScripts = new List<MonoBehaviour>();
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
    public GameObject heliPosition;

    // Event Tornado
    public bool eventTornado = false;
    public bool eventTornadoDone = false;

    // Event Bomb & Police
    public bool eventBomb = false;
    public short eventBombPlaced = 0;
    public bool eventPolice = false;
    public bool eventBombDone = false;

    // Event Reset
    public bool eventReset = false;

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

    public void SubscribeScriptToGameEventUpdates(MonoBehaviour pScript) {
        eventSubscribedScripts.Add(pScript);
    }

    public void DeSubscribeScriptToGameEventUpdates(MonoBehaviour pScript)
    {
        while(eventSubscribedScripts.Contains(pScript)) {
            eventSubscribedScripts.Remove(pScript);
    
        }
    }

    public void PlayerActivatedEvent() {
        foreach(MonoBehaviour _script in eventSubscribedScripts) {
            _script.Invoke("gameEventUpdated", 0);
        }

    }

}
