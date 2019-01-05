using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using Vuforia;

public class ActionCardTracker : MonoBehaviour, ITrackableEventHandler
{

    private TrackableBehaviour mTrackableBehaviour;
    private GameObject PlaceEvent;
    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        PlaceEvent = GameObject.Find("PlaceEvent");
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED)
        {

            switch (gameObject.name)
            {
                case "TargetFire":
                    GameController.Instance.isFireTargetTracked = true;
                    break;
                case "TargetUfo":
                    GameController.Instance.isUfoTargetTracked = true;
                    break;
                default:
                    break;
            }


        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
               newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            switch (gameObject.name)
            {
                case "TargetFire":
                    GameController.Instance.isFireTargetTracked = false;
                    break;
                case "TargetUfo":
                    GameController.Instance.isUfoTargetTracked = false;
                    break;
                default:
                    break;
            }
        }
    }


}