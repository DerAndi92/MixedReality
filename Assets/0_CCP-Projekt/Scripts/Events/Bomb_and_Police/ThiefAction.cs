using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefAction : MonoBehaviour
{
    //Welche Bombe wird vom Dieb platziert?
    public GameObject bomb;

    // Welcher Dieb löst das Event aus?
    public GameObject thief;

    private AudioSource audioPlaceBomb;

    private void Start()
    {
        audioPlaceBomb = GameObject.Find("PlaceBomb").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Wenn der richtige Dieb das Event triggert, wird stoppt er und kniet sich hin.
        // Die Bombe wird nach einer Sekunde platziert und nach iner weiteren Sekunde rennt der Dieb weg.
        if (other.gameObject == thief)
        {
            thief.GetComponent<Moveit>().stop();
            thief.GetComponent<Waypoints>().isMoving = false;
            thief.GetComponent<Crouch>().start();

            Invoke("placeBomb", 1f); 
        }

    }

    private void placeBomb()
    {
        // Bombe platziert
        audioPlaceBomb.Play();
        bomb.SetActive(true);
        GameController.Instance.eventBombPlaced++;

        Invoke("runAway", 1f);
    }

    private void runAway()
    {
        // Dieb rennt weg
        thief.GetComponent<Moveit>().start();
        thief.GetComponent<Moveit>().run();
        thief.GetComponent<Moveit>().speed = 2.4f;
        thief.GetComponent<Waypoints>().isMoving = true;
    }
}
