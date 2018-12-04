using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using Vuforia;

public class FirefighterTracker : MonoBehaviour, ITrackableEventHandler
{

    private TrackableBehaviour mTrackableBehaviour;


    private GameObject fireCar;
    private PlayableDirector fireTimeline;

    private Waypoints waypoint;
    void Start()
    {
        fireCar = GameObject.Find("fireCar");
        fireTimeline = GameObject.Find("fireCar_timeline").GetComponent<PlayableDirector>();
        fireTimeline.Stop();
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED)
        {
            fireTimeline.Play();
        }
        else
        {
        }
    }

   
}