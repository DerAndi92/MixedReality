using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Waypoints : MonoBehaviour
{

    public GameObject[] waypoints;
    public float[] rotations;
    public string[] collisionOn;

    public bool isMoving;
    public bool repeat;
    public bool backward;

    public float speed;
    public float waypointRadius;
    public string collisionGroup;

    private float acutalSpeed;
    private bool isBackward;
    private bool collision = false;
    private int current = 0;
    private List<Collider> collissions = new List<Collider>();

    private void Start()
    {
        acutalSpeed = speed;
    }

    void Update()
    {
        // Should it move?
        if (isMoving)
        {

            // Move it, if there is no Collision
            if (!collision)
            {
                // Move each frame
                transform.position = Vector3.MoveTowards(transform.position, waypoints[current].transform.position, Time.deltaTime * acutalSpeed);
                if (Vector3.Distance(waypoints[current].transform.position, transform.position) < waypointRadius)
                {

                    // Reached waypoint
                    // Is it moving backwards?
                    if (!isBackward)
                    {
                        // Rotate if there is a rotation array
                        if (rotations.Length > 0) transform.Rotate(0, rotations[current], 0);

                        // Set next waypoint
                        current++;
                        if (current >= waypoints.Length)
                        {
                            // Reached last waypoint. What now? Go Backward? Repeat it? Or nothing?
                            if (backward)
                            {
                                transform.Rotate(0, 180, 0);
                                isBackward = true;
                                current -= 2;
                            }
                            else if (repeat)
                            {
                                current = 0;
                            }
                            else
                            {
                                isMoving = false;
                            }
                        }
                    }
                    // It is moving backwards!
                    else
                    {
                        // Rotate if there is a rotation array, but the rotation is +180 (backward)
                        if (rotations.Length > 0 && rotations[current] != 0) transform.Rotate(0, rotations[current] + 180, 0);

                        // Reached last waypoint? What now? Repeat it? Or nothing?
                        if (current == 0)
                        {
                            if (repeat)
                            {
                                current = 0;
                                transform.Rotate(0, 180, 0);
                                isBackward = false;
                            }
                            else
                            {
                                isMoving = false;
                            }
                        }
                        // If it is not the last waypoint, move to the next!
                        else
                        {
                            current--;
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only detect a collision if the collisionGroup of the Collider ist in the collisionOn array
        if (other.gameObject.GetComponent<Waypoints>() && collisionOn.Contains(other.gameObject.GetComponent<Waypoints>().collisionGroup))
        {
            // Only detect a collision on the front side of the collider
            Vector3 direction = other.transform.position - transform.position;
            if (Vector3.Dot(transform.forward, direction) < 0)
            {
                collision = true;
                collissions.Add(other);

                // Reset Speed to default
                this.resetActualSpeed();
            }
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if(collissions.Contains(other))
        {
            collissions.Remove(other);
        }
        
        if(collissions.Count() == 0) collision = false;
    }

    public void setActualSpeed(float speed)
    {
        this.acutalSpeed = speed;
    }

    public void resetActualSpeed()
    {
        this.acutalSpeed = speed;
    }
}
