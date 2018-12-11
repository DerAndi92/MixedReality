using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCollider : MonoBehaviour {

    private GameObject testObject;

	// Use this for initialization
	void Awake () {
		testObject = GameObject.Find("Building_Cinema");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if(gameObject.tag == "triggerTarget") {
		 testObject.SetActive(false);
		 }
     
	}
}
