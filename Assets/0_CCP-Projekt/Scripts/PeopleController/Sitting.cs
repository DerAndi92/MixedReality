using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sitting : MonoBehaviour
{

    private Animator animator;

    // Soll die Animation bereits aktiv sein?
    public bool sitting = true;


    void Start()
    {
        this.animator = this.GetComponent<Animator>();
        if (this.animator != null)
        {
            if (this.sitting) this.start();
        }
    }

    // Animation stopppen (Idle aktiv)
    public void stop()
    {
        sitting = false;
        this.animator.SetInteger("Animation_int", 0);
    }

    // Animation starten
    public void start()
    {
        sitting = true;
        this.animator.SetFloat("Speed_f", 0.0f);
        this.animator.SetBool("Grounded", true);
        this.animator.SetBool("Static_b", true);
        this.animator.SetInteger("Animation_int", 9);
        this.animator.SetInteger("WeaponType_int", 0);
        this.animator.SetBool("Crouch_b", false);
    }
}
