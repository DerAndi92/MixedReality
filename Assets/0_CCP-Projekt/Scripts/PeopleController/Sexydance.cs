using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sexydance : MonoBehaviour
{

    private Animator animator;

    // Soll die Animation bereits aktiv sein?
    public bool dance = true;

    // Use this for initialization
    void Start()
    {
        this.animator = this.GetComponent<Animator>();
        if (this.animator != null)
        {
            if (this.dance) this.start();
        }
    }

    // Animation stopppen (Idle aktiv)
    public void stop()
    {
        dance = false;
        this.animator.SetInteger("Animation_int", 0);
    }

    // Animation starten
    public void start()
    {
        dance = true;
        this.animator.SetInteger("Animation_int", 4);
        this.animator.SetFloat("Speed_f", 0);
        this.animator.SetBool("Grounded", true);
        this.animator.SetBool("Static_b", false);
        this.animator.SetInteger("WeaponType_int", 0);
        this.animator.SetBool("Crouch_b", false);
    }
}
