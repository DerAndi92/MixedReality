using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventUfo : MonoBehaviour
{

    private GameObject ufo;
    private GameObject currentObject;
    private GameObject[] objects;
    private int current;
    private float distance;
    private bool isObjectPrepared;
    private int speed;
    ParticleSystem explosionUfo;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        current = 0; 
        ufo = GameObject.Find("Ufo");
        explosionUfo = GameObject.Find("Ufo_Explosion").GetComponent<ParticleSystem>();
        timer = 0;

        isObjectPrepared = false;
        if (objects == null)
            objects = GameObject.FindGameObjectsWithTag("Part of Ufo Action");

    }

    // Update is called once per frame
    void Update()
    {
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
                    currentObject.transform.position = Vector3.MoveTowards(currentObject.transform.position, ufo.transform.position, 1.5f);
                    if ((ufo.transform.position.y - currentObject.transform.position.y) < 0.5) {
                        Debug.Log("Aufgesaugt");
                        isObjectPrepared = false;
                        Destroy(currentObject);
                        current++;
                    }
                }
            }
        } else {
            explosionUfo.Play();
            timerUpdate();
            if(timer > 2) {
                ufo.transform.localScale = new Vector3(0, 0, 0);
            }

        }

    }
    void timerUpdate()
    {
        timer += Time.deltaTime;
    }

}
