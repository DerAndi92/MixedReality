using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using Vuforia;
using UnityEngine.SceneManagement;

public class ResetTarget : MonoBehaviour, ITrackableEventHandler
{
    private TrackableBehaviour mTrackableBehaviour;
    private AudioSource destructionAudioSource;
    private GameObject explosion;
    private AudioSource explosionAudio;

    // Start is called before the first frame update
    void Start()
    {
        destructionAudioSource = GameObject.Find("DestructionAudioSource").GetComponent<AudioSource>();
        explosion = GameObject.Find("FinalExplosion");
        explosionAudio = explosion.GetComponent<AudioSource>();
        explosion.SetActive(false);
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartReset() {
        explosion.SetActive(true);
        Invoke("DoReset", 2);

    }

    void DoReset()
    {
        SceneManager.LoadScene("City_Town");

    }
    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        // Ein Marker wird entdeckt
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED)
        {


            destructionAudioSource.Play();
            Invoke("StartReset", destructionAudioSource.clip.length);
           
            GameController.Instance.isEventInPlace = false;
            GameController.Instance.isRescueInPlace = false;

            GameController.Instance.isFireTargetTracked = false;
            GameController.Instance.isUfoTargetTracked = false;
            GameController.Instance.isFireCarTargetTracked = false;
            GameController.Instance.isHelicopterTargetTracked = false;
            GameController.Instance.isTornadoTargetTracked = false;
            GameController.Instance.isBombTargetTracked = false;
            GameController.Instance.isPoliceTargetTracked = false;

            // Event Fire & FireCar
            GameController.Instance.eventFire = false;
            GameController.Instance.eventFireCar = false;
            GameController.Instance.isFireCleared = false;
            GameController.Instance.isWaterThrowerRight = false;

            // Event Ufo & Heli
            GameController.Instance.eventUfo = false;
            GameController.Instance.eventHelicopter = false;
            GameController.Instance.ufoIsShot = false;

            // Event Tornado
            GameController.Instance.eventTornado = false;

            // Event Bomb & Police
            GameController.Instance.eventBomb = false;
            GameController.Instance.eventPolice = false;

        }
    }
}