using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talking : MonoBehaviour {
    private Animator animator;

    // Wie oft soll die Pose gewechselt werden?
    public float timeLeft = 4.0f;

    // Welche Poste ist aktiv?
    public int activeAnimation = 2;

    void Start () {
        this.animator = this.GetComponent<Animator>();
        if (this.animator != null)
        {
            // Aktiviere Gesprächs-Animation mit Start-Pose
            this.animator.SetFloat("Speed_f", 0.0f);
            this.animator.SetBool("Grounded", true);
            this.animator.SetBool("Static_b", true);
            this.animator.SetInteger("Animation_int", activeAnimation);
            this.animator.SetBool("Crouch_b", false);
        }
    }
	
	void Update () {

        // Ändere die Pose wenn die entsprechende Zeit erreicht ist.
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            switch (activeAnimation)
            {
                case 1:
                    activeAnimation = 2;
                    this.animator.SetInteger("Animation_int", activeAnimation);
                    break;
                case 2:
                    activeAnimation = 1;
                    this.animator.SetInteger("Animation_int", activeAnimation);
                    break;
                default:
                    this.animator.SetInteger("Animation_int", activeAnimation);
                    break;
            }
            timeLeft = 4.0f;
        }

    }
}
