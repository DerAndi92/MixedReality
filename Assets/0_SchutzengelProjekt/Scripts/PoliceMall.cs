using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceMall : MonoBehaviour
{
    public GameObject police;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject == police)
        {
            GameController.Instance.eventPoliceAtMall = true;
        }

    }
}
