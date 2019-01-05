using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using Vuforia;

public class RescueCardTracker : MonoBehaviour, ITrackableEventHandler
{

    private TrackableBehaviour mTrackableBehaviour;
    private PlayableDirector fireCarTimeline;

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
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED) {
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
                    default:
                        break;
                }
            }
        
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
             newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            switch (gameObject.name)
            {
                case "TargetFireCar":
                    GameController.Instance.isFireCarTargetTracked = false;
                    break;
                case "TargetHelicopter":
                    GameController.Instance.isHelicopterTargetTracked = false;
                    break;
                default:
                    break;
            }
        }

    }

   
}