using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsOnHips : MonoBehaviour {

    private Animator animator;
    public bool hip = true;

    // Use this for initialization
    void Start () {
        this.animator = this.GetComponent<Animator>();
        if (this.animator != null)
        {
            if (this.hip) this.start();
        }
    }

    public void stop()
    {
        hip = false;
        this.animator.SetInteger("Animation_int", 0);
    }

    public void start()
    {
        this.animator.SetFloat("Speed_f", 0.0f);
        this.animator.SetBool("Grounded", true);
        this.animator.SetBool("Static_b", true);
        this.animator.SetBool("Crouch_b", false);
        this.animator.SetInteger("Animation_int", 2);
    }

}


