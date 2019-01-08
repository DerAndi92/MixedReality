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
    private Light light;
    private float lightStart;

    private bool peopleRan = false;
    private bool tornadoBuilt = false;
    private bool lightDim = false;
    private bool done = false;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        tornado = GameObject.Find("TornadoObject");
        light = GameObject.Find("Directional Light").GetComponent<Light>();
        lightStart = light.intensity;
        swimmingWaypoints = GameObject.Find("Swimming_Waypoints_Tornado").GetComponent<Waypoints>();
        tornadoParticle = tornado.GetComponent<ParticleSystem>();
        tornadoParticle.Pause(true);
    }

    // Update is called once per frame
    void Update()
    {

        if (!done && GameController.Instance.eventTornado)
        {

            // Wenn gestartet, wird das Licht gedimmt
            if (!lightDim)
            {
                if (light.intensity >= 0.1)
                    light.intensity -= Time.deltaTime * 0.4f;
                else
                    lightDim = true;
            }

            // Wenn Licht gedimmt, wird Tornado aufgebaut
            if (lightDim && !tornadoBuilt) {
                tornadoParticle.Play(true);
                tornado.GetComponent<Waypoints>().isMoving = true;
                tornado.GetComponent<TornadoDestroyer>().start = true;

                tornadoBuilt = true;
                timer = 0;
            }

            // Nach einer Sekunde laufen die Leute weg 
            if(!peopleRan && tornadoBuilt && timer > 1 )
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

                
                peopleRan = true;
                timer = 0;
            }

            if(peopleRan && !GameObject.Find("TornadoObject"))
            {
                if (light.intensity < lightStart)
                {
                    light.intensity += Time.deltaTime * 0.4f;
                }
                else
                { 
                    done = true;
                    GameController.Instance.eventTornado = false;
                }
            }
            
        }


        if (!done)
        {
            timer += Time.deltaTime;
        }
        
    }
}
