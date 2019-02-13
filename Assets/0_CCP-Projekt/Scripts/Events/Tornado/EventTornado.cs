using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTornado : MonoBehaviour
{
    public GameObject[] runningPeople;
    public GameObject[] swimmingPeople;

    private GameObject tornado;
    private ParticleSystem tornadoParticle;
    private Waypoints swimmingWaypoints;
    private Light cityLight;
    private float lightStart;

    private AudioSource audioTornado;
    private AudioSource audioTornadoScream;

    private bool peopleRan = false;
    private bool tornadoBuilt = false;
    private bool lightDim = false;
    private float timer = 0;

    void Start()
    {
        tornado = GameObject.Find("TornadoObject");
        cityLight = GameObject.Find("Directional Light").GetComponent<Light>();
        swimmingWaypoints = GameObject.Find("Swimming_Waypoints_Tornado").GetComponent<Waypoints>();
        tornadoParticle = tornado.GetComponent<ParticleSystem>();

        audioTornado = GameObject.Find("AudioTornado").GetComponent<AudioSource>();
        audioTornadoScream = GameObject.Find("AudioTornadoScream").GetComponent<AudioSource>();
        audioTornado.Stop();
        audioTornadoScream.Stop();

        lightStart = cityLight.intensity;
        tornadoParticle.Pause(true);
    }

    // Update is called once per frame
    void Update()
    {

        if (!GameController.Instance.eventTornadoDone && GameController.Instance.eventTornado)
        {

            // Wenn gestartet, wird das Licht gedimmt
            if (!lightDim)
            {
                if(!audioTornado.isPlaying) audioTornado.Play();
                if (cityLight.intensity >= 0.1)
                    cityLight.intensity -= Time.deltaTime * 0.4f;
                else
                    lightDim = true;
            }

            // Wenn Licht gedimmt, wird Tornado aufgebaut
            else if (lightDim && !tornadoBuilt) {
                tornadoParticle.Play(true);
                tornado.GetComponent<Waypoints>().isMoving = true;
                tornado.GetComponent<TornadoDestroyer>().start = true;
                
                tornadoBuilt = true;
                timer = 0;
            }

            // Nach einer Sekunde laufen die Leute weg 
            else if(!peopleRan && tornadoBuilt && timer > 1 )
            {
                // Chillende Leute rennen weg
                foreach (GameObject p in runningPeople)
                {
                    p.GetComponent<Moveit>().start();
                    p.GetComponent<Moveit>().run();
                    p.GetComponent<Waypoints>().isMoving = true;
                }

                // Leute im Wasser schwimmen raus und laufen weg!
                foreach (GameObject p in swimmingPeople)
                {
                    p.GetComponent<Moveit>().start();
                    p.GetComponent<Moveit>().run();
                    p.transform.Rotate(90, 0 ,0);
                    p.transform.position = new Vector3(p.transform.position.x, -0.5f, p.transform.position.z);
                    p.GetComponent<Waypoints>().changeWaypoints(swimmingWaypoints.waypoints, swimmingWaypoints.rotations, swimmingWaypoints.speed, true, false, false, true);
                }

                audioTornadoScream.Play();
                peopleRan = true;
                timer = 0;
            }

            // Wenn alle Leute weggelaufen sind, wird geprüft, ob der Tornado noch da ist (dieser verschwindet, wenn alle Waypoints abgelaufen wurden)
            // Ist das der Fall, wird das Licht wieder normal, der Ton gestopppt und das Event beendet.
            else if(peopleRan && !GameObject.Find("TornadoObject"))
            {
                if (cityLight.intensity < lightStart)
                {
                    cityLight.intensity += Time.deltaTime * 0.4f;
                }
                else
                {
                    GameController.Instance.eventTornadoDone = true;
                    GameController.Instance.eventTornado = false;
                    audioTornado.Stop();
                }
            }


            if (!GameController.Instance.eventTornadoDone)
            {
                timer += Time.deltaTime;
            }
        }
    }
}
