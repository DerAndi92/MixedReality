using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsOnHips : MonoBehaviour {

    private Animator animator;

    // Soll die Animation bereits aktiv sein?
    public bool hip = true;

    void Start () {
        this.animator = this.GetComponent<Animator>();
        if (this.animator != null)
        {
            if (this.hip) this.start();
        }
    }

    // Animation stopppen (Idle aktiv)
    public void stop()
    {
        hip = false;
        this.animator.SetInteger("Animation_int", 0);
    }

    // Animation starten
    public void start()
    {
        this.animator.SetFloat("Speed_f", 0.0f);
        this.animator.SetBool("Grounded", true);
        this.animator.SetBool("Static_b", true);
        this.animator.SetBool("Crouch_b", false);
        this.animator.SetInteger("Animation_int", 2);
    }

}


