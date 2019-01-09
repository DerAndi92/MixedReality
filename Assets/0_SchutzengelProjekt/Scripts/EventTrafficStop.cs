using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EventTrafficStop : MonoBehaviour
{
    public float checkInterval = 3;
    private float delta = 0;

    private List<Collider> cars = new List<Collider>();

    void Update()
    {
        delta += Time.deltaTime;

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
        Debug.Log("TRIGGERD____");
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
