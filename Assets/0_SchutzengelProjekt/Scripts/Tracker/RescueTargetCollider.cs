using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueTargetCollider : MonoBehaviour
{

    GameObject test;
    // Update is called once per frame
    void Start()
    {
        test = GameObject.Find("van_mesh_red_TEST");
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "RescueTarget":
                GameController.Instance.isRescueInPlace = true;
                if (GameController.Instance.isFireCarTargetTracked)
                {
                    GameController.Instance.eventFireCar = true;
                }
                else if (GameController.Instance.isHelicopterTargetTracked)
                {
                    GameController.Instance.eventHelicopter = true;
                }
                test.transform.localScale = new Vector3(20, 20, 20);
                break;
            default:
                break;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "RescueTarget":
                GameController.Instance.isRescueInPlace = false;
                test.transform.localScale = new Vector3(1, 1, 1);
                break;
            default:
                break;
        }

    }
}
