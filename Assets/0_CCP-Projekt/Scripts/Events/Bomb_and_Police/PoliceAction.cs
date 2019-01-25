using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceAction : MonoBehaviour
{
    public GameObject bomb;
    public GameObject police;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject == police)
        {
            police.GetComponent<Moveit>().stop();
            police.GetComponent<Waypoints>().isMoving = false;
            police.GetComponent<Crouch>().start();

            Invoke("removeBomb", 2f);
        }

    }

    private void removeBomb()
    {
        // Bomb placed
        Destroy(bomb);
        Invoke("runAway", 0.5f);
    }

    private void runAway()
    {
        // Police run away
        GameController.Instance.eventBombRemoved++;
        police.GetComponent<Moveit>().start();
        police.GetComponent<Waypoints>().isMoving = true;
    }
}
