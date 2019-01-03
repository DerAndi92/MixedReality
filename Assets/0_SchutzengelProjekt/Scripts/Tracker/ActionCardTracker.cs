using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using Vuforia;

public class ActionCardTracker : MonoBehaviour, ITrackableEventHandler
{

    private TrackableBehaviour mTrackableBehaviour;

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
                Debug.Log("target erkant!");
				switch (gameObject.name)
				{
					case "TargetFire":
                    Debug.Log("TargetFire erkant!");
                        if (!GameController.Instance.eventFire)
                        {
                            GameController.Instance.eventFire = true;
                            GameController.Instance.PlayerActivatedEvent();
                        }
                        break;
					case "TargetUfo":
                        if (!GameController.Instance.eventUfo)
                        {
                            GameController.Instance.eventUfo = true;
                        }
                        break;
                default:
						break;
				}
        
        }
        
    }

   
}