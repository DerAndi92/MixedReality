using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;

using UnityEngine;


public class EventFire : MonoBehaviour {

    private PlayableDirector fireTimeline;

    // Use this for initialization
    void Start () {
        GameController.Instance.SubscribeScriptToGameEventUpdates(this);
        fireTimeline = GameObject.Find("fire_timeline").GetComponent<PlayableDirector>();
        fireTimeline.Stop();
    }

    void OnDestroy() {
        GameController.Instance.DeSubscribeScriptToGameEventUpdates(this);
    }

    void gameEventUpdated() {
        if (GameController.Instance.eventFire)
        {
            if (!GameController.Instance.isFire)
            {
                GameController.Instance.isFire = true;
                fireTimeline.Play();

                if (GameController.Instance.fireCareOnEvent)
                {
                    fireTimeline.Stop();
                }
            }
        }
    }


}
