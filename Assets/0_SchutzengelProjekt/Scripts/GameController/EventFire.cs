using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;

using UnityEngine;


public class EventFire : MonoBehaviour {

    private PlayableDirector fireTimeline;
    private GameObject[] houses;
    private int houseNumber = 0;
    private GameObject fireGroup;

    // Use this for initialization
    void Start () {
        GameController.Instance.SubscribeScriptToGameEventUpdates(this);
        fireTimeline = GameObject.Find("fire_timeline").GetComponent<PlayableDirector>();
        fireGroup = GameObject.Find("FireGroup");
        fireTimeline.Stop();
        if (houses == null)
            houses = GameObject.FindGameObjectsWithTag("house");
    }

    void OnDestroy() {
        GameController.Instance.DeSubscribeScriptToGameEventUpdates(this);
    }

    void gameEventUpdated() {
        Debug.Log("YEAH");
        if (GameController.Instance.eventFire)
        {
            if(houses.Length > houseNumber) {
            fireTimeline.Stop();
            if (houses[houseNumber] != null) {
                fireGroup.transform.position = houses[houseNumber].transform.position;
                fireTimeline.Play();
            
            }
            houseNumber ++;
        }
        }
        
    }


}
