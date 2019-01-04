using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveit: MonoBehaviour {

    private Animator animator;
    public bool walk = true;

	// Use this for initialization
	void Start () {
        this.animator = this.GetComponent<Animator>();
        if(this.animator != null)
        {
            this.animator.SetFloat("Speed_f", 0.0f);
            this.animator.SetBool("Grounded", true);
            this.animator.SetBool("Static_b", true);
            this.animator.SetInteger("Animation_int", 0);
            this.animator.SetInteger("WeaponType_int", 0);
            if (this.walk) this.go();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void stop()
    {
        this.animator.SetFloat("Speed_f", 0.0f);
    }

    public void go()
    {
        this.animator.SetFloat("Speed_f", 0.4f);
    }
}
