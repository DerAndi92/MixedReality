using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventUfo : MonoBehaviour
{

    private GameObject ufo;
    private GameObject currentObject;
    private GameObject startPosition;
    private GameObject endPosition;
    private GameObject spotLight;
    private GameObject[] objects;

    private AudioSource ufoAbsorbAudio;
    private AudioSource ufoBackgroundAudio;
    private AudioSource explosion1Audio;
    private AudioSource explosion2Audio;
    private AudioSource objectSound;
    private int current;
    private int speed;
    private float distance;
    private float timer;
   
    private bool isObjectPrepared = false;
    private bool isUfoAtPosition = false;
    private bool isUfoActive = false;
    private bool isSoundPlaying = false;
    private bool isExplosio1Fired = false;
    private bool isExplosio2Fired = false;

    ParticleSystem explosionUfo1;
    ParticleSystem explosionUfo2;


    void Start()
    {
        FindGameObjects();
        StopComponents();
        current = 0;
        timer = 0;
        ufo.SetActive(false);
    }

    void Update()
    {
        if (GameController.Instance.eventUfo) {
            // solagne das Ufo noch nicht an der Startposition ist, wird es dorthin bewegt
            if (!isUfoAtPosition) {
                if (!isUfoActive) {
                    ufo.SetActive(true);
                    ufoBackgroundAudio.Play(0);
                    isUfoActive = true;
                }
                speed = 10;
                ufo.transform.position = Vector3.MoveTowards(ufo.transform.position, startPosition.transform.position, Time.deltaTime * speed);
                if (Vector3.Distance(ufo.transform.position, startPosition.transform.position) < 0.5) {
                    isUfoAtPosition = true;
                }
            } else {
                // solange das Ufo noch nicht abgeschossen wurde, saugt es alle Objekte auf mit dem entsprechendem Tag
                if(!GameController.Instance.ufoIsShot) {
                    if (current < objects.Length) {
                        if(!isObjectPrepared) {
                            PrepareObject();
                        }

                        MoveUfoToObject();

                        if (Vector2.Distance(new Vector2(ufo.transform.position.x, ufo.transform.position.z), new Vector2(currentObject.transform.position.x, currentObject.transform.position.z)) < 0.5) {
                            if(!isSoundPlaying)
                            {
                                PlayAudio();
                            }
                            // Aufsaugen des Objektes
                            currentObject.transform.position = Vector3.MoveTowards(currentObject.transform.position, ufo.transform.position, 0.5f);
                            if ((ufo.transform.position.y - currentObject.transform.position.y) < 0.5) {
                                PrepareNextObject();
                            }
                        }
                    } else {
                        // nachdem alle Objekte aufgesaugt wurden, fliegt das Ufo weg und wird ausgeblendet
                        ufo.transform.position = Vector3.MoveTowards(ufo.transform.position, endPosition.transform.position, Time.deltaTime * speed);
                        if(Vector3.Distance(ufo.transform.position, endPosition.transform.position) < 0.5) {
                            ufo.SetActive(false);
                            GameController.Instance.eventUfo = false;
                            ufoBackgroundAudio.Stop();
                        }
                    }
                } else {
                    // Wenn der Helikopter geschossen hat, dann werden die zwei Explosionen gestartet und das Ufo ausgeblendet
                    TimerUpdate();
                    if (!isExplosio1Fired)
                    {
                        explosionUfo1.Play();
                        isExplosio1Fired = true;
                        explosion1Audio.Play();
                    }
                    if (!isExplosio2Fired && timer > 0.5)
                    {
                        explosionUfo2.Play();
                        isExplosio2Fired = true;
                        explosion2Audio.Play();
                        ufo.transform.localScale = new Vector3(0, 0, 0);
                        ufoBackgroundAudio.Stop();
                        spotLight.SetActive(false);
                    }

                }
            }
        }

    }

    void FindGameObjects()
    {
        ufo = GameObject.Find("Ufo");
        startPosition = GameObject.Find("UfoStartPoint");
        endPosition = GameObject.Find("UfoEndPoint");
        spotLight = GameObject.Find("UfoSpotLight");
        explosionUfo1 = GameObject.Find("Ufo_Explosion1").GetComponent<ParticleSystem>();
        explosionUfo2 = GameObject.Find("Ufo_Explosion2").GetComponent<ParticleSystem>();
        ufoAbsorbAudio = GameObject.Find("UfoAbsorbAudioSource").GetComponent<AudioSource>();
        ufoBackgroundAudio = GameObject.Find("UfoBackgroundRumbleAudioSource").GetComponent<AudioSource>();
        explosion1Audio = GameObject.Find("Explosion1SoundSource").GetComponent<AudioSource>();
        explosion2Audio = GameObject.Find("Explosion2SoundSource").GetComponent<AudioSource>();
        if (objects == null)
            objects = GameObject.FindGameObjectsWithTag("Part of Ufo Action");
    }

    void StopComponents()
    {
        explosionUfo1.Stop();
        explosionUfo2.Stop();
        ufoAbsorbAudio.Stop();
    }

    void PrepareObject()
    {
        currentObject = objects[current];
        objectSound = currentObject.GetComponent<AudioSource>();
        isObjectPrepared = true;
    }
    void PrepareNextObject()
    {
        isObjectPrepared = false;
        Destroy(currentObject);
        isSoundPlaying = false;
        current++;
    }

    void PlayAudio()
    {
        ufoAbsorbAudio.Play(0);
        isSoundPlaying = true;
        if (objectSound != null)
        {
            objectSound.Play();
        }
    }

    void MoveUfoToObject()
    {
        distance = Vector2.Distance(new Vector2(ufo.transform.position.x, ufo.transform.position.z), new Vector2(currentObject.transform.position.x, currentObject.transform.position.z));
        // neues Posistion des Ziels -> wird durchgängig neu abgefragt, das manche Objekte sich bewegen
        Vector3 ufoNewPosition = new Vector3(currentObject.transform.position.x, startPosition.transform.position.y, currentObject.transform.position.z);
        if (distance > 3)
        {
            speed = 6;
        }
        else
        {
            speed = 2;
        }
        ufo.transform.position = Vector3.MoveTowards(ufo.transform.position, ufoNewPosition, Time.deltaTime * speed);
    }

    void TimerUpdate()
    {
        timer += Time.deltaTime;
    }

}
