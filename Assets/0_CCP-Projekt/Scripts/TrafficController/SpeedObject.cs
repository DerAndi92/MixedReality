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


	void Start () {
        delta = 0;
	}
	

	void Update () {

        delta += Time.deltaTime;
    }

    // Wenn ein Auto über ein Speed Object fährt, wird geprüft, wie lang es her ist, 
    // dass ein anderes Auto über die gleiche Plattform gefahren ist. Basierend auf dem Unterschied
    // wird die Geschwindigkeit erhöht.
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
    
