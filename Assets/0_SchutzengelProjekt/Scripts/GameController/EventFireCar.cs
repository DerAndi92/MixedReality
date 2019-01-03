using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class EventFireCar : MonoBehaviour {

    private GameObject fireCar;
    private Waypoints waypointsFireCar;
    // Use this for initialization
    void Start()
    {
        fireCar = GameObject.Find("fireCar");
        waypointsFireCar = fireCar.GetComponent<Waypoints>();

        GameController.Instance.SubscribeScriptToGameEventUpdates(this);
    }

    void OnDestroy()
    {
        GameController.Instance.DeSubscribeScriptToGameEventUpdates(this);
    }

    void gameEventUpdated()
    {
        Debug.Log("YEEEEEAH______");
        if(!GameController.Instance.eventFire && !GameController.Instance.fireCareOnEvent) {
            waypointsFireCar.current = 0;
            waypointsFireCar.isMoving = true;
            GameController.Instance.fireCareOnEvent = true;
        }

    }
}

