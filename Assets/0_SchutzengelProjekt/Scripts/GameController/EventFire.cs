using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EventFire : MonoBehaviour {

    private PlayableDirector fireTimeline;
    private GameObject[] objects;
    private int objectNumber = 0;
    private GameObject fireGroup;
    float timer = 25;
    private bool isBuildingDeactivated = false;

    // Use this for initialization
    void Start () {
        GameController.Instance.SubscribeScriptToGameEventUpdates (this);
        fireTimeline = GameObject.Find ("fire_timeline").GetComponent<PlayableDirector> ();
        fireGroup = GameObject.Find ("FireGroup");
        fireTimeline.Stop ();
        if (objects == null)
            objects = GameObject.FindGameObjectsWithTag ("Part of Action");
        foreach (GameObject ob in objects) {
            Debug.Log (ob.name);
        }

    }

    void OnDisabled () {
        GameController.Instance.DeSubscribeScriptToGameEventUpdates (this);
    }

    void Update () {
        if (GameController.Instance.eventFire) {

            if (timer < 25) {
                if(timer > 20 && !isBuildingDeactivated) {
                    disableBuilding();
                    isBuildingDeactivated = true;
                }
                timerUpdate ();
            } else {
                timerReset ();
            }
        }
    }

    void timerUpdate () {
        timer += Time.deltaTime;
    }
    void timerReset () {
isBuildingDeactivated = false;
        startFire ();
        timer = 0;
        objectNumber++;
    }

    void startFire () {
        if (objects.Length > objectNumber) {
            fireTimeline.Stop ();
            if (objects[objectNumber] != null) {
                ObjectActionHandler other = objects[objectNumber].GetComponent<ObjectActionHandler> ();
                Vector3 test= new Vector3 (other.FirePositionX / 2.8f, fireGroup.transform.position.y, other.FirePositionZ / 2.9f);
                fireGroup.transform.position = new Vector3(0,0,0);
                Debug.Log ("FEUER POSITION _________________");
                Debug.Log ("test" + test);
                Debug.Log ("other.FirePositionZ: " + other.FirePositionZ);
                                fireGroup.transform.position = test;


                Debug.Log (fireGroup.transform.position);
                fireTimeline.Play ();
            }
        }
    }

    void disableBuilding () {
        GameObject building = GameObject.Find (objects[objectNumber - 1].name + "_Building");
        building.SetActive (false);
    }

    void gameEventUpdated () {

    }

}