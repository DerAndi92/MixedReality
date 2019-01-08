using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using Vuforia;

public class ActionCardTracker : MonoBehaviour, ITrackableEventHandler
{

    private TrackableBehaviour mTrackableBehaviour;

    // DIESES SKRIPT WIRD AUF JEDEM ACTION CARD MARKER AUSGEFÜHRT
    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        // Ein Marker wird entdeckt
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED)
        {

            // Prüfen ob der Marker zu einem der Events gehört
            // Falls ja wird der Flag gesetzt, dass es getracked wird
            // Außerdem wird das Event gestartet, wodurch die Event Skripte aktiv werden, die darauf warten, dass der Flag gesetzt wird
            switch (gameObject.name)
            {
                case "TargetFire":
                    GameController.Instance.isFireTargetTracked = true;
                    GameController.Instance.eventFire = true;
                    break;
                case "TargetUfo":
                    GameController.Instance.isUfoTargetTracked = true;
                    GameController.Instance.eventUfo = true;
                    break;
                case "TargetTornado":
                    GameController.Instance.isTornadoTargetTracked = true;
                    GameController.Instance.eventTornado = true;
                    break;
                case "TargetBomb":
                    GameController.Instance.isBombTargetTracked = true;
                    break;
                default:
                    break;
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


}