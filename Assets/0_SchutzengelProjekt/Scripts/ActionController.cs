using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour {

    public float firstActionAfterMapTracked = 5.0f;
    public bool actionOneStarted = false;
    ParticleSystem fireParticle;
    ParticleSystem fireParticle2;
    int animateFire = 0;

    public int number;
    // Use this for initialization
    void Start () {
        fireParticle = GameObject.Find("fireParticle").GetComponent<ParticleSystem>();
        fireParticle2 = GameObject.Find("fireParticle2").GetComponent<ParticleSystem>();
        fireParticle.Stop();
        fireParticle2.Stop();
        number = 1;

    }
	
	// Update is called once per frame
	void Update () {


        switch (number)
        {
            case 0:
                Debug.Log("0");
                break;
            case 1:
                firstActionAfterMapTracked -= Time.deltaTime;
                if (firstActionAfterMapTracked <= 0.0f)
                {
                    if (animateFire == 0)
                    {
                        fireParticle.Play();
                        animateFire += 1;
                    } else if (animateFire == 1) {
                        fireParticle2.Play();
                    }
                    actionOneStarted = true;
                    firstActionAfterMapTracked = 5.0f;

                }
                break;
            case 2:
                Debug.Log("3");
                break;
            default:
                Debug.Log("def");
                break;
        }





    }


    void startParticlesystem(ParticleSystem pS) {
        fireParticle.Play();
    }

}
