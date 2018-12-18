using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class EventFireCar : MonoBehaviour {

    private PlayableDirector fireCarTimeline;

    // Use this for initialization
    void Start()
    {
        GameController.Instance.SubscribeScriptToGameEventUpdates(this);
        fireCarTimeline = GameObject.Find("fireCar_timeline").GetComponent<PlayableDirector>();
        fireCarTimeline.Stop();
    }

    void OnDestroy()
    {
        GameController.Instance.DeSubscribeScriptToGameEventUpdates(this);
    }

    void gameEventUpdated()
    {
        if (GameController.Instance.eventFire)
        {
            fireCarTimeline.Play();

        }
     }
}

