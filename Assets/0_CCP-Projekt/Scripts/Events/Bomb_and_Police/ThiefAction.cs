using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefAction : MonoBehaviour
{
    public GameObject bomb;
    public GameObject thief;
    private AudioSource audioPlaceBomb;

    private void Start()
    {
        audioPlaceBomb = GameObject.Find("PlaceBomb").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
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
        // Bomb placed
        audioPlaceBomb.Play();
        bomb.SetActive(true);
        GameController.Instance.eventBombPlaced++;

        Invoke("runAway", 1f);
    }

    private void runAway()
    {
        // Theifs run away
        thief.GetComponent<Moveit>().start();
        thief.GetComponent<Moveit>().run();
        thief.GetComponent<Moveit>().speed = 2.4f;
        thief.GetComponent<Waypoints>().isMoving = true;
    }
}
