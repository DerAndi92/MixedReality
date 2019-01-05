using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class EventFireCar : MonoBehaviour {

    private GameObject fireCar;
    private AudioSource sirene;
    private Waypoints waypointsFireCar;
    // Use this for initialization
    void Start()
    {
        fireCar = GameObject.Find("fireCar");
        waypointsFireCar = fireCar.GetComponent<Waypoints>();
        sirene = GameObject.Find("FireCarSireneAudioSource").GetComponent<AudioSource>();

        GameController.Instance.SubscribeScriptToGameEventUpdates(this);
    }

    void OnDisable()
    {
        GameController.Instance.DeSubscribeScriptToGameEventUpdates(this);
    }

    void gameEventUpdated()
    {
        if(!GameController.Instance.eventFire && !GameController.Instance.fireCareOnEvent) {
            waypointsFireCar.current = 0;
            waypointsFireCar.isMoving = true;
            GameController.Instance.fireCareOnEvent = true;
        }

    }
}

