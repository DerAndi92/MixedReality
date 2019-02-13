using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceMall : MonoBehaviour
{
    public GameObject police;

    // Event wird ausgelöst, wenn die Polizei in der Mall angekommen ist, um danach weitere Aktionen auszulösen.
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject == police)
        {
            GameController.Instance.eventPoliceAtMall = true;
        }

    }
}
