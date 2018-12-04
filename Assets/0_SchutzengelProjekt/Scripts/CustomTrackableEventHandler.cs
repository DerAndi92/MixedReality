using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class CustomTrackableEventHandler : MonoBehaviour, ITrackableEventHandler {

    private TrackableBehaviour mTrackableBehaviour;
    private bool isTracked = false;
    private AudioSource audioSource;
    private AudioLowPassFilter alpf;
    private Rotator rotator;

    // Use this for initialization
    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        audioSource = GetComponent<AudioSource>();
        alpf = GetComponent<AudioLowPassFilter>();

        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }
            // Update is called once per frame
    void Update () {
        if (isTracked && audioSource.isPlaying)
        {
            Vector3 pos = Camera.main.transform.position;
            float distance = pos.magnitude;
            alpf.cutoffFrequency = distance * 5000.0f;
        }
            

    }

    void ITrackableEventHandler.OnTrackableStateChanged (TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {

            isTracked = true;
            if (!audioSource.isPlaying)
                audioSource.Play();
        }

        else if (newStatus == TrackableBehaviour.Status.NOT_FOUND &&
                    previousStatus == TrackableBehaviour.Status.TRACKED)
        {
            isTracked = false;

            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }


}
