using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHelicopter : MonoBehaviour
{

    private GameObject helicopter;
    private GameObject ufo;
    private GameObject landingPosition;
    private GameObject heliNoEventPoint;

    private Animator helicopterAnimator;
    private int heliStartSeconds = 3;
    private Vector3 heliStartPosition;
    private Vector3 heliLookAtPoint;
    private float timer;
    private bool isHeliStarted = false;
    private bool isHeliLanding = false;
    private bool isHeliPrepared = false;
    private bool isHeliLooking = false;
    private string heliOnEvent = "";


    // Start is called before the first frame update
    void Start()
    {
        helicopter = GameObject.Find("Helicopter");
        ufo = GameObject.Find("Ufo");
        landingPosition = GameObject.Find("HelicopterLandingPoint");
        heliNoEventPoint = GameObject.Find("HeliNoEventPoint");
        helicopter.SetActive(false);
        helicopterAnimator = helicopter.GetComponent<Animator>();
        helicopterAnimator.speed = 0;
        heliStartPosition = new Vector3(helicopter.transform.position.x, helicopter.transform.position.y + 40, helicopter.transform.position.z);
        heliLookAtPoint = new Vector3(helicopter.transform.position.x + 30, helicopter.transform.position.y + 40, helicopter.transform.position.z);
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.eventHelicopter)
        {
            if(!isHeliPrepared) {
                helicopter.SetActive(true);
                helicopterAnimator.speed = 1;
                isHeliPrepared = true;
                if ( GameController.Instance.eventUfo)
                {
                    heliOnEvent = "ufo";
                }
                else
                {
                    heliOnEvent = "noEvent";
                }
            }
            if (GameController.Instance.eventUfo && heliOnEvent == "ufo")
            {
                if ((heliStartPosition.y - helicopter.transform.position.y) >= 1 && !isHeliStarted)
                {
                    helicopter.transform.position = Vector3.MoveTowards(helicopter.transform.position, heliStartPosition, Time.deltaTime * 10);
                }
                else
                {
                    isHeliStarted = true;

                    if (Vector3.Distance(helicopter.transform.position, ufo.transform.position) > 20 && !GameController.Instance.ufoIsShot)
                    {

                        helicopter.transform.position = Vector3.MoveTowards(helicopter.transform.position, ufo.transform.position, Time.deltaTime * 10);
                        Quaternion _lookRotation = Quaternion.LookRotation((ufo.transform.position - helicopter.transform.position).normalized);
                        helicopter.transform.rotation = Quaternion.Slerp(helicopter.transform.rotation, _lookRotation, Time.deltaTime * 2);
                    }
                    else
                    {
                        GameController.Instance.ufoIsShot = true;
                        timerUpdate();
                        if (timer > 2)
                        { 
                            landingHeli();
                        }

                    }


                }
            }
            else if (heliOnEvent == "noEvent")
            {
                if ((heliStartPosition.y - helicopter.transform.position.y) >= 1 && !isHeliStarted)
                {
                    helicopter.transform.position = Vector3.MoveTowards(helicopter.transform.position, heliStartPosition, Time.deltaTime * 10);
                }
                else
                {
                    isHeliStarted = true;

                    if (Vector3.Distance(helicopter.transform.position, heliNoEventPoint.transform.position) > 1 && !isHeliLooking)
                    {

                        helicopter.transform.position = Vector3.MoveTowards(helicopter.transform.position, heliNoEventPoint.transform.position, Time.deltaTime * 10);
                        Quaternion _lookRotation = Quaternion.LookRotation((heliNoEventPoint.transform.position - helicopter.transform.position).normalized);
                        helicopter.transform.rotation = Quaternion.Slerp(helicopter.transform.rotation, _lookRotation, Time.deltaTime * 2);
                    }
                    else
                    {
                        isHeliLooking = true;
                        timerUpdate();
                        if (timer > 2)
                        {
                            landingHeli();
                        }

                    }
                }
            }
        }

    }

    void landingHeli()
    {
        if (!isHeliLanding)
        {
            helicopter.transform.position = Vector3.MoveTowards(helicopter.transform.position, heliStartPosition, Time.deltaTime * 10);
            Quaternion _lookRotation = Quaternion.LookRotation((heliLookAtPoint - helicopter.transform.position).normalized);
            helicopter.transform.rotation = Quaternion.Slerp(helicopter.transform.rotation, _lookRotation, Time.deltaTime * 2);
        }
        if (Vector3.Distance(helicopter.transform.position, heliStartPosition) < 1 || isHeliLanding)
        {
            isHeliLanding = true;
            helicopter.transform.position = Vector3.MoveTowards(helicopter.transform.position, landingPosition.transform.position, Time.deltaTime * 10);
            if (Vector3.Distance(helicopter.transform.position, landingPosition.transform.position) < 1)
            {
                GameController.Instance.eventHelicopter = false;
                isHeliStarted = false;
                isHeliLanding = false;
                isHeliPrepared = false;
                isHeliLooking = false;
                helicopter.SetActive(false);
                heliOnEvent = "";
                timer = 0;
            }
        }
    }

    void timerUpdate()
    {
        timer += Time.deltaTime;
    }
}
