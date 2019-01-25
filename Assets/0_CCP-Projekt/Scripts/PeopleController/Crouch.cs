using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{

    private Animator animator;
    public bool crouch = true;

    // Use this for initialization
    void Start()
    {
        this.animator = this.GetComponent<Animator>();
        if (this.animator != null)
        {
            if (this.crouch) this.start();
        }
    }

    public void stop()
    {
        crouch = false;
        this.animator.SetBool("Crouch_b", false);
    }

    public void start()
    {
        this.animator.SetFloat("Speed_f", 0.0f);
        this.animator.SetBool("Grounded", true);
        this.animator.SetBool("Static_b", true);
        this.animator.SetInteger("Animation_int", 0);
        this.animator.SetBool("Crouch_b", true);
    }

}


