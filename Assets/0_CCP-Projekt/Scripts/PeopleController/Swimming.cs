﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swimming : MonoBehaviour
{

    private Animator animator;

    // Use this for initialization
    void Start()
    {
        this.animator = this.GetComponent<Animator>();
        if (this.animator != null)
        {
            this.animator.SetFloat("Speed_f", 0.0f);
            this.animator.SetBool("Grounded", false);
            this.animator.SetBool("Static_b", true);
            this.animator.SetInteger("Animation_int", 0);
            this.animator.SetInteger("WeaponType_int", 0);
            this.animator.SetBool("Crouch_b", false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}