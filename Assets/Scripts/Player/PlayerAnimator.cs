using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimator : MonoBehaviour
{
    private Animator anim;
    private CharacterController charController;
    private CharacterMovement charMovement;
    private bool moving = true; //simple control

    // Start is called before the first frame update
    void Start()
    {
        //set value of references using getcomponent
        anim = GetComponent<Animator>();
        charController = GetComponentInParent<CharacterController>();
        charMovement = GetComponentInParent<CharacterMovement>();  
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            //set value of speed parameter as a function of the ratio
            //between our current speed (velocity) and max possible pseed
            //this returns a value between 0 to Stopped, and 1 to full speed
            //which tracks perfectly with our blend tree
            anim.SetFloat("Speed", charController.velocity.magnitude / charMovement.speed);

            gameObject.transform.localPosition = Vector3.zero;
        }
    }

    public void setTrigger(string trig)
    {
        anim.SetTrigger(trig);
    }
}