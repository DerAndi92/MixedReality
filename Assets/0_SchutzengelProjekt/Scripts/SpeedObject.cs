using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedObject : MonoBehaviour {

    public float SecondsLong = 3;
    public float SecondsShort = 2;
    public float SpeedBoostLong = 12;
    public float SpeedBoostShort = 10;

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
            else
            {
                wp.resetActualSpeed();
            }

            delta = 0;
        }
    }
}
    
