using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHelicopter : MonoBehaviour
{

    private GameObject helicopter;
    private GameObject ufo;
    private GameObject noEventFlyPoint;

    private Animator helicopterAnimator;
    private AudioSource heliSound;

    private Vector3 heliStartPosition;
    private Vector3 heliLookAtPoint;
    private Vector3 landingFieldPosition;
    private Vector3 landingPosition;

    private float timer;
    private bool isHeliStarted = false;
    private bool isHeliLanding = false;
    private bool isHeliPrepared = false;
    private bool isAnimationAndSound = false;
    private bool isHeliAtFlyingPosition = false;
    private string heliOnEvent = "";

    void Start()
    {
        FindObjects();
        GetPositionsForHeli();

        timer = 0;
        helicopterAnimator.speed = 0;
        helicopter.SetActive(false);
    }

    void Update()
    {        
   
        if (GameController.Instance.eventHelicopter)
        {
            if (!isHeliPrepared) {
                // Helikopter auf Dach positionieren
                GetPositionsForHeli();
                helicopter.transform.position = landingFieldPosition;
               
                if(!isAnimationAndSound) {
                    heliSound.Play();
                    helicopter.SetActive(true);
                    helicopterAnimator.speed = 1;
                    isAnimationAndSound = true;
                }
                
                // Setzten des Events so, dass der Helikopter bei Aktivierung des Ufo, während er fliegt, wieder zurück zum
                // Landeplatz fliegt und nicht das Ufo angreift -> Man muss Heli erneut losschicken
                if (GameController.Instance.eventUfo)
                    heliOnEvent = "ufo";
                else
                    heliOnEvent = "noEvent";

                // Heli startet nach zwei Sekunden
                if (timer < 2) { 
                    TimerUpdate();
                }
                else { 
                    timer = 0;
                    isHeliPrepared = true;
                }

            } 
            else if (heliOnEvent == "ufo")
            {
                DoHeliAction(ufo.transform.position, 10);

            }
            else if (heliOnEvent == "noEvent")
            {
                DoHeliAction(noEventFlyPoint.transform.position, 1);
            }
        } else {
            // platzieren, des Helikopters auf dem Helikopter-Landeplatz, sobald Gebäude getrackted wird
            if(GameController.Instance.isHelicopterTargetTracked ) {
                helicopter.SetActive(true);
                landingFieldPosition = GameObject.Find("HeliPosition").transform.position;
                helicopter.transform.position = landingFieldPosition;
            } else {
                helicopter.SetActive(false);
            }
        }

    }
    void DoHeliAction(Vector3 flyingPosition, int distance) {
        // Heli fliegt zur Startposition
        if ((heliStartPosition.y - helicopter.transform.position.y) >= 1 && !isHeliStarted)
        {
            helicopter.transform.position = Vector3.MoveTowards(helicopter.transform.position, heliStartPosition, Time.deltaTime * 6);
        }
        else
        {
            isHeliStarted = true;
            if (Vector3.Distance(helicopter.transform.position, flyingPosition) > distance && !isHeliAtFlyingPosition)
            {
                MoveHelicopterTo(flyingPosition);
            }
            else
            {
                isHeliAtFlyingPosition = true;
                if (heliOnEvent == "ufo")
                {
                    // löst die Explosionen beim Ufo aus im Ufoscript
                    GameController.Instance.ufoIsShot = true;
                }
                LandingHeli();
            }
        }
    }
    void FindObjects() {
        heliSound = GameObject.Find("HelicopterAudio").GetComponent<AudioSource>();
        helicopter = GameObject.Find("Helicopter");
        ufo = GameObject.Find("Ufo");
        noEventFlyPoint = GameObject.Find("HeliNoEventPoint");
        helicopterAnimator = helicopter.GetComponent<Animator>();
    }

    void GetPositionsForHeli() {
        landingFieldPosition = GameObject.Find("HeliPosition").transform.position;
        heliStartPosition = new Vector3(helicopter.transform.position.x, helicopter.transform.position.y + 10, helicopter.transform.position.z);
        heliLookAtPoint = new Vector3(helicopter.transform.position.x + 20, helicopter.transform.position.y + 10, helicopter.transform.position.z);
        landingPosition = helicopter.transform.position;
    }

    void MoveHelicopterTo(Vector3 position) {
        helicopter.transform.position = Vector3.MoveTowards(helicopter.transform.position, position, Time.deltaTime * 10);
        Quaternion _lookRotation = Quaternion.LookRotation((position - helicopter.transform.position).normalized);
        helicopter.transform.rotation = Quaternion.Slerp(helicopter.transform.rotation, _lookRotation, Time.deltaTime * 2);
    }

    void LandingHeli()
    {
        TimerUpdate();
        if (timer > 2)
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
                helicopter.transform.position = Vector3.MoveTowards(helicopter.transform.position, landingPosition, Time.deltaTime * 6);
                if (Vector3.Distance(helicopter.transform.position, landingPosition) < 1)
                {
                    helicopterAnimator.speed = 0;
                    GameController.Instance.eventHelicopter = false;
                    isHeliStarted = false;
                    isHeliLanding = false;
                    isHeliPrepared = false;
                    isHeliAtFlyingPosition = false;
                    isAnimationAndSound = false;
                    heliSound.Stop();
                    helicopter.SetActive(false);
                    heliOnEvent = "";
                    timer = 0;
                }
            }
        }
    }

    void TimerUpdate()
    {
        timer += Time.deltaTime;
    }
}
