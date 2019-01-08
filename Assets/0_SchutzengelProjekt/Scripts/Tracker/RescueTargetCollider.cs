using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueTargetCollider : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        // Der Collider vom Parkplatz wird getriggert
        if(other.tag == "RescueTarget")
        {
            GameController.Instance.isRescueInPlace = true;

            // Prüfen welches Rettungsfahrzeug den Collider getriggert hat, indem geschaut wird, welcher FahrzeugMarker aktuell im Bild ist und getracked wird.
            if (GameController.Instance.isFireCarTargetTracked)
            {
                GameController.Instance.eventFireCar = true;
            }
            else if (GameController.Instance.isHelicopterTargetTracked)
            {
                GameController.Instance.eventHelicopter = true;
            }
            else if (GameController.Instance.isPoliceTargetTracked)
            {
                GameController.Instance.eventPolice = true;
            }
        }


    }
    private void OnTriggerExit(Collider other)
    {
        // FahrzeugMarker wird wieder weggenommen
        // Flag das ein Fahrzeug gelegt wurde wird false gesetzt
        // Die Events selbst werden nicht auf false gesetzt, das passiert bei den Eventskripten selbst, wenn diese fertig sind
        if (other.tag == "RescueTarget")
        {
            GameController.Instance.isRescueInPlace = false;
        }

    }
}
