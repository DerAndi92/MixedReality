using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveit: MonoBehaviour {

    private Animator animator;

    // Soll die Animation bereits aktiv sein?
    public bool walk = true;

    // Wie schnell laufen?
    public float speed = 0.4f;

	// Use this for initialization
	void Start () {
        this.animator = this.GetComponent<Animator>();
        if(this.animator != null)
        {
            if (this.walk) this.start();
        }
	}

    // Animation stopppen (Idle aktiv)
    public void stop()
    {
        walk = false;
        this.animator.SetFloat("Speed_f", 0.0f);
    }

    // Animation starten
    public void start()
    {
        if(this.animator)
        {
            walk = true;
            this.animator.SetFloat("Speed_f", speed);
            this.animator.SetBool("Grounded", true);
            this.animator.SetBool("Static_b", true);
            this.animator.SetInteger("Animation_int", 0);
            this.animator.SetBool("Crouch_b", false);
            this.animator.SetInteger("WeaponType_int", 0);
        }
    }

    // Laufgeschwindigkeit erhöhen
    public void run()
    {
        if (this.animator)
        {
            this.animator.SetFloat("Speed_f", 0.6f);
        }  
    }
}
