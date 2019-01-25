using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedObject : MonoBehaviour {

    public float SecondsLong = 2.5f;
    public float SecondsShort = 1.5f;
    public float SecondsTiny = 0.8f;
    public float SpeedBoostLong = 13;
    public float SpeedBoostShort = 11;
    public float SpeedBoostTiny = 9;

    private float delta;

	// Use this for initialization
	void Start () {
        delta = 0;
	}
	
	// Update is called once per frame
	void Update () {

        delta += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Waypoints wp = other.GetComponent<Waypoints>();
        if (wp != null) {
            if (delta >= SecondsLong)
            {
                wp.setActualSpeed(SpeedBoostLong);

            }
            else if(delta >= SecondsShort)
            {
                wp.setActualSpeed(SpeedBoostShort);
            }
            else if (delta >= SecondsTiny)
            {
                wp.setActualSpeed(SpeedBoostTiny);
            }
            else
            {
                wp.resetActualSpeed();
            }

            delta = 0;
        }
    }
}
    
