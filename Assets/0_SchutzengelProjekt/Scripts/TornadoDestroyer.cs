using UnityEngine;
using System.Collections;

public class TornadoDestroyer : MonoBehaviour
{
    public bool start = false;
    private GameObject PullOBJ;
    private GameObject vortexMid;
    public float PullSpeed;

    private void Start()
    {
        vortexMid = GameObject.Find("Vortex_Mid");
    }

    public void OnTriggerStay(Collider coll)
    {
        
        if (start && coll.gameObject.tag == "Tornado_Object")
        {
            PullOBJ = coll.gameObject;

            PullOBJ.transform.position = Vector3.MoveTowards(PullOBJ.transform.position, vortexMid.transform.position, PullSpeed * Time.deltaTime);
            if (Vector3.Distance(PullOBJ.transform.position, vortexMid.transform.position) < 0.1f)
            {
                Destroy(PullOBJ);
            }
        }
    }

}
