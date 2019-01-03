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
				switch (gameObject.name)
				{
					case "FireCarTarget":
                        GameController.Instance.trackedFireCarTarget = true;
                        GameController.Instance.Invoke("PlayerActivatedEvent",0);
						break;
					case "HelicopterTarget":
                        GameController.Instance.eventHelicopter = true;
                        break;
					default:
						break;
				}
        
        } 
        
    }

   
}