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
        fireCarTimeline = GameObject.Find("fireCar_timeline").GetComponent<PlayableDirector>();
        fireCarTimeline.Stop();

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
						fireCarTimeline.Play();
						break;
					case "...":
						break;
					default:
						break;
				}
        
        }
        
    }

   
}