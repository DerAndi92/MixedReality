using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveit: MonoBehaviour {

    private Animator animator;
    public bool walk = true;
    public float speed = 0.4f;

	// Use this for initialization
	void Start () {
        this.animator = this.GetComponent<Animator>();
        if(this.animator != null)
        {
            if (this.walk) this.start();
        }
	}

    public void stop()
    {
        walk = false;
        this.animator.SetFloat("Speed_f", 0.0f);
    }

    public void start()
    {
        walk = true;
        this.animator.SetFloat("Speed_f", speed);
        this.animator.SetBool("Grounded", true);
        this.animator.SetBool("Static_b", true);
        this.animator.SetInteger("Animation_int", 0);
        this.animator.SetInteger("WeaponType_int", 0);
    }

    public void run()
    {
        this.animator.SetFloat("Speed_f", 0.6f);
    }
}
