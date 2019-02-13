using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : MonoBehaviour
{

    private Animator animator;

    // Soll die Animation bereits aktiv sein?
    public bool gun = true;

    void Start()
    {
        this.animator = this.GetComponent<Animator>();
        if (this.animator != null)
        {
            if (this.gun) this.start();
        }
    }

    // Animation stopppen (Idle aktiv)
    public void stop()
    {
        gun = false;
        this.animator.SetInteger("WeaponType_int", 0);
    }

    // Animation starten
    public void start()
    {
        gun = true;
        this.animator.SetFloat("Speed_f", 0.0f);
        this.animator.SetBool("Grounded", true);
        this.animator.SetBool("Static_b", true);
        this.animator.SetBool("Crouch_b", false);
        this.animator.SetInteger("Animation_int", 0);
        this.animator.SetInteger("WeaponType_int", 8);
    }
}
