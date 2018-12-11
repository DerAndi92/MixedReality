using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using Vuforia;

public class ActionCardTracker : MonoBehaviour, ITrackableEventHandler
{

    private TrackableBehaviour mTrackableBehaviour;
    private PlayableDirector fireTimeline;

    void Start()
    {
        fireTimeline = GameObject.Find("fire_timeline").GetComponent<PlayableDirector>();
        fireTimeline.Stop();
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
					case "FireTarget":
						fireTimeline.Play();
						break;
					case "...":
						break;
					default:
						break;
				}
        
        }
        
    }

   
}