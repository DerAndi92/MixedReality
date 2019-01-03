using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHelicopter : MonoBehaviour
{

    private GameObject helicopter;
    private Animator helicopterAnimator;
    private int heliStartSeconds = 3;
    private Vector3 heliStartPosition;
    private float timer;
    private GameObject ufo;
    private bool isHeliStarted;
    private bool isUfoExploded;


    // Start is called before the first frame update
    void Start()
    {
        helicopter = GameObject.Find("Helicopter");
        ufo = GameObject.Find("Ufo");

        helicopterAnimator = helicopter.GetComponent<Animator>();
        helicopterAnimator.speed = 0;
        heliStartPosition = new Vector3(helicopter.transform.position.x, helicopter.transform.position.y + 40, helicopter.transform.position.z);
        isHeliStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.eventHelicopter)
        {
            helicopterAnimator.speed = 1;
            if ((heliStartPosition.y - helicopter.transform.position.y) >= 1 && !isHeliStarted)
            {
                helicopter.transform.position = Vector3.MoveTowards(helicopter.transform.position, heliStartPosition, 1);
            }
            else
            {
                isHeliStarted = true;

                if (Vector3.Distance(helicopter.transform.position, ufo.transform.position) > 20 && !GameController.Instance.ufoIsShot)
                {

                    helicopter.transform.position = Vector3.MoveTowards(helicopter.transform.position, ufo.transform.position, 1);
                }
                else
                {
                    GameController.Instance.ufoIsShot = true;

                }
                Quaternion _lookRotation = Quaternion.LookRotation((ufo.transform.position - helicopter.transform.position).normalized);
                helicopter.transform.rotation = Quaternion.Slerp(helicopter.transform.rotation, _lookRotation, Time.deltaTime * 2);

            }
        }

    }
    void timerUpdate()
    {
        timer += Time.deltaTime;
    }
}
