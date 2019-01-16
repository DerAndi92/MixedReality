using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using Vuforia;
using System.Linq;

public class ActionCardTracker : MonoBehaviour, ITrackableEventHandler
{

    private TrackableBehaviour mTrackableBehaviour;
    private AudioSource activationSound;

    // DIESES SKRIPT WIRD AUF JEDEM ACTION CARD MARKER AUSGEFÜHRT
    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }

        activationSound = GameObject.Find("ActivationSound").GetComponent<AudioSource>();
        activationSound.Stop();
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        // Ein Marker wird entdeckt
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED)
        {

            string invokeEvent = "";

            // Prüfen ob der Marker zu einem der Events gehört
            // Falls ja wird der Flag gesetzt, dass es getracked wird
            // Prüfen ob das Event abgesielt werden soll. Falls ja wird der Name der Startmethode in invokeEvent geschrieben
            switch (gameObject.name)
            {
                case "TargetFire":
                    GameController.Instance.isFireTargetTracked = true;
                    if (!GameController.Instance.eventFire && !GameController.Instance.eventFireDone)
                        invokeEvent = "startFireEvent";
                    break;

                case "TargetUfo":
                    GameController.Instance.isUfoTargetTracked = true;
                    Debug.Log(!GameController.Instance.eventUfo && !GameController.Instance.eventUfoDone);
                    if (!GameController.Instance.eventUfo && !GameController.Instance.eventUfoDone){
                        GameController.Instance.eventUfoDone = true;
                        invokeEvent = "startUfoEvent";
                    }
                    break;

                case "TargetTornado":
                    GameController.Instance.isTornadoTargetTracked = true;
                    if(!GameController.Instance.eventTornado && !GameController.Instance.eventTornadoDone)
                        invokeEvent = "startTornadoEvent";
                    break;

                case "TargetBomb":
                    GameController.Instance.isBombTargetTracked = true;
                    if (!GameController.Instance.eventBomb && !GameController.Instance.eventBombDone)
                        invokeEvent = "startBombEvent";
                    break;

                default:
                    break;
            }

            // Sound abspielen und Event danach aktivieren, wenn das Event gestartet werden soll!
            if (invokeEvent != "") {
                activationSound.Play();
                Invoke(invokeEvent, activationSound.clip.length);
            }

        }

        // Marker wird wieder aus dem Bild genommen
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
               newStatus == TrackableBehaviour.Status.NO_POSE)
        {

            // Prüfen welcher Marker von welchem Event entfernt wurde 
            // Flag das der Marker getracked wird, wird auf false gesetzt
            // Die Events selbst werden nicht auf false gesetzt, das passiert bei den Eventskripten selbst, wenn diese fertig sind
            switch (gameObject.name)
            {
                case "TargetFire":
                    GameController.Instance.isFireTargetTracked = false;
                    break;
                case "TargetUfo":
                    GameController.Instance.isUfoTargetTracked = false;
                    break;
                case "TargetTornado":
                    GameController.Instance.isTornadoTargetTracked = false;
                    break;
                case "TargetBomb":
                    GameController.Instance.isBombTargetTracked = false;
                    break;
                default:
                    break;
            }
        }
    }

    void startFireEvent()
    {
        GameController.Instance.eventFire = true;
    }

    void startUfoEvent()
    {
        GameController.Instance.eventUfo = true;
    }

    void startTornadoEvent()
    {
        GameController.Instance.eventTornado = true;
    }

    void startBombEvent()
    {
        GameController.Instance.eventBomb = true;
    }

}