using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTargetCollider : MonoBehaviour {

    GameObject test;
	// Update is called once per frame
	void Start () {
        test = GameObject.Find("tree_large_mesh (3)_TEST");
	}

	void OnTriggerEnter(Collider other) {
        switch (other.tag)
        {
            case "EventTarget":
                GameController.Instance.isEventInPlace = true;
                if(GameController.Instance.isFireTargetTracked)
                {
                    GameController.Instance.eventFire = true;
                } else if(GameController.Instance.isUfoTargetTracked)
                {
                    GameController.Instance.eventUfo = true;
                }
                test.transform.localScale = new Vector3(20, 20, 20);
                break;
            default:
                break;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "EventTarget":
                GameController.Instance.isEventInPlace = false;
                test.transform.localScale = new Vector3(1, 1, 1);
                break;
            default:
                break;
        }
       
    }
}
