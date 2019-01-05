using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventUfo : MonoBehaviour
{

    private GameObject ufo;
    private GameObject currentObject;
    private GameObject startPosition;
    private GameObject endPosition;
    private GameObject[] objects;

    private AudioSource ufoAbsorbAudio;
    private AudioSource ufoBackgroundAudio;
    private AudioSource explosion1Audio;
    private AudioSource explosion2Audio;
   
    private int current;
    private int speed;
    private float distance;
    private float timer;
   
    private bool isObjectPrepared = false;
    private bool isUfoAtPosition = false;
    private bool isUfoActive = false;
    private bool isSoundPlaying = false;
    private bool isUfoPreseted = false;
    private bool isExplosio1Fired = false;
    private bool isExplosio2Fired = false;

    ParticleSystem explosionUfo1;
    ParticleSystem explosionUfo2;


    // Start is called before the first frame update
    void Start()
    {
        current = 0; 
        ufo = GameObject.Find("Ufo");
        startPosition = GameObject.Find("UfoStartPoint");
        endPosition = GameObject.Find("UfoEndPoint");

        explosionUfo1 = GameObject.Find("Ufo_Explosion1").GetComponent<ParticleSystem>();
        explosionUfo1.Stop();
        explosionUfo2 = GameObject.Find("Ufo_Explosion2").GetComponent<ParticleSystem>();
        explosionUfo2.Stop();

        timer = 0;
        ufoAbsorbAudio = GameObject.Find("UfoAbsorbAudioSource").GetComponent<AudioSource>();
        ufoBackgroundAudio = GameObject.Find("UfoBackgroundRumbleAudioSource").GetComponent<AudioSource>();
        explosion1Audio = GameObject.Find("Explosion1SoundSource").GetComponent<AudioSource>();
        explosion2Audio = GameObject.Find("Explosion2SoundSource").GetComponent<AudioSource>();


        ufoAbsorbAudio.Stop();
        ufo.SetActive(false);

        if (objects == null)
            objects = GameObject.FindGameObjectsWithTag("Part of Ufo Action");

    }

    // Update is called once per frame
    void Update()
    {
        if(!isUfoPreseted)
        {
            ufoBackgroundAudio.Play(0);
            isUfoPreseted = true;
        }
        if (GameController.Instance.eventUfo) {
            if (!isUfoAtPosition) {
                if(!isUfoActive) {
                    ufo.SetActive(true);
                    isUfoActive = true;
                }
                speed = 10;
                ufo.transform.position = Vector3.MoveTowards(ufo.transform.position, startPosition.transform.position, Time.deltaTime * speed);
                if(Vector3.Distance(ufo.transform.position, startPosition.transform.position) < 0.5) {
                    isUfoAtPosition = true;
                }
            } else {

                if(!GameController.Instance.ufoIsShot) {
                    if (current < objects.Length) {
                        if(!isObjectPrepared) {
                            currentObject = objects[current];
                            isObjectPrepared = true;
                        }
                        distance = Vector2.Distance(new Vector2(ufo.transform.position.x, ufo.transform.position.z), new Vector2(currentObject.transform.position.x, currentObject.transform.position.z));
                        Vector3 ufoNewPosition = new Vector3(currentObject.transform.position.x, ufo.transform.position.y, currentObject.transform.position.z);
                        if(distance > 3) {
                            speed = 10;
                        } else {
                            speed = 5;
                        } 
                        ufo.transform.position = Vector3.MoveTowards(ufo.transform.position, ufoNewPosition, Time.deltaTime * speed);

                        if (Vector2.Distance(new Vector2(ufo.transform.position.x, ufo.transform.position.z), new Vector2(currentObject.transform.position.x, currentObject.transform.position.z)) < 0.5) {
                            if(!isSoundPlaying)
                            {
                                ufoAbsorbAudio.Play(0);
                                isSoundPlaying = true;
                            }
                            currentObject.transform.position = Vector3.MoveTowards(currentObject.transform.position, ufo.transform.position, 1.5f);
                            if ((ufo.transform.position.y - currentObject.transform.position.y) < 0.5) {
                                Debug.Log("Aufgesaugt");
                                isObjectPrepared = false;
                                Destroy(currentObject);
                                isSoundPlaying = false;
                                current++;
                            }
                        }
                    } else {

                        ufo.transform.position = Vector3.MoveTowards(ufo.transform.position, endPosition.transform.position, Time.deltaTime * speed);
                        if(Vector3.Distance(ufo.transform.position, endPosition.transform.position) < 0.5) {
                            ufo.SetActive(false);
                            GameController.Instance.eventUfo = false;
                            ufoBackgroundAudio.Stop();
                        }
                    }
                } else {
                    timerUpdate();
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
                    }

                }
            }
        }

    }
    void timerUpdate()
    {
        timer += Time.deltaTime;
    }

}
