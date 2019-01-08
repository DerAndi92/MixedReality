using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using Vuforia;

public class RescueCardTracker : MonoBehaviour, ITrackableEventHandler
{

    private TrackableBehaviour mTrackableBehaviour;

    // DIESES SKRIPT WIRD AUF JEDEM RESCUE CARD MARKER AUSGEFÜHRT
    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus,TrackableBehaviour.Status newStatus)
    {
        // Ein Marker wird entdeckt
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED) {

            // Prüfen ob der Marker zu einem der Resuce Events gehört
            // Falls ja wird der Flag gesetzt, dass es getracked wird
            // Als nächstes muss der Marker noch in den Collider für die Rescue Events gehalten werden, damit das Event startet (siehe RescueTargetCollider Skript)
            if (GameController.Instance.isEventInPlace)
            {
                switch (gameObject.name)
                {
                    case "TargetFireCar":
                        GameController.Instance.isFireCarTargetTracked = true;
                        break;
                    case "TargetHelicopter":
                        GameController.Instance.isHelicopterTargetTracked = true;
                        break;
                    case "TargetPolizei":
                        GameController.Instance.isPoliceTargetTracked = true;
                        break;
                    default:
                        break;
                }
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
                case "TargetFireCar":
                    GameController.Instance.isFireCarTargetTracked = false;
                    break;
                case "TargetHelicopter":
                    GameController.Instance.isHelicopterTargetTracked = false;
                    break;
                case "TargetPolizei":
                    GameController.Instance.isPoliceTargetTracked = false;
                    break;
                default:
                    break;
            }
        }

    }

   
}