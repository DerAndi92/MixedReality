using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EventTrafficStop : MonoBehaviour
{

    // Dieses Skript wird benutzt, um den verkehr an bestimmten Punkten anhalten, wenn ein Event aktiv ist.

    public float checkInterval = 3;
    private float delta = 0;

    private List<Collider> cars = new List<Collider>();

    void Update()
    {
        delta += Time.deltaTime;

        // Wenn ein Event aktiv war, nun aber beendet wurde, dadurch Autos jedoch gestoppt wurden, werden diese Autos nun wieder aktiviert.
        if (delta >= checkInterval && !GameController.Instance.isTrafficStopped && cars.Count() > 0)
        {
            delta = 0;
            foreach (Collider c in cars)
            {
                c.GetComponent<Waypoints>().isMoving = true;
            }
            cars.Clear();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // Wenn ein Auto den Trigger aktivert und ein bestimmtes Event aktiv ist, wird das Auto gestoppt.
        if(GameController.Instance.isTrafficStopped) {
            Waypoints wp = other.GetComponent<Waypoints>();
            if (wp != null)
            {
                wp.isMoving = false;
                cars.Add(other);  
            }
        }
    }
}
