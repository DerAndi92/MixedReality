using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastARCamera : MonoBehaviour
{

    private RaycastHit hit;
    private bool hitObject = false;
    private GameObject truck;

    public GameObject obj;
    // Use this for initialization
    void Start()
    {
        if (this.gameObject.name.Equals("PizzaShop"))
        {
            truck = GameObject.Find("HotdogTruck");
            truck.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Transform cam = Camera.main.transform;
        Ray ray = new Ray(cam.position, cam.forward);
        Debug.DrawRay(ray.origin, ray.direction * 1000.0f, Color.green, 10, false);
        Debug.Log("Looking for TTTESSST");
        if (Physics.Raycast(ray, out hit, 1000.0f))
        {
            Debug.Log("Looking for " + this.transform.name + " hitting " + hit.transform.name);
            if (hit.transform.name == this.transform.name)
            {
                hitObject = true;
            }
            else
            {
                hitObject = false;
            }
        }

        if (hitObject)
        {
            if (this.gameObject.name.Equals("PizzaShop"))
                truck.SetActive(true);
        }
        else
        {
            if (this.gameObject.name.Equals("PizzaShop"))
                truck.SetActive(false);
        }
    }

   
}
