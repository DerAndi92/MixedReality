using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceAction : MonoBehaviour
{

    // welche Bombe wird vom Polizist entfernt?
    public GameObject bomb;

    // Welcher Polizist läst das Event aus?
    public GameObject police;

    // Wenn das der Trigger erreicht wird, setzt sich Polizist vor die Bombe und entschäft diese
    // Nach 2 Sekunden wird die Bombe entfernt und der Polizist rennt nach weiteren 0.5 Sekunden zurück zum Auto
    private void OnTriggerEnter(Collider other)
    {
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
        // Bomben zerstört
        Destroy(bomb);
        Invoke("runAway", 0.5f);
    }

    private void runAway()
    {
        // Polizist rennt zurück zum Auto
        GameController.Instance.eventBombRemoved++;
        police.GetComponent<Moveit>().start();
        police.GetComponent<Waypoints>().isMoving = true;
    }
}
