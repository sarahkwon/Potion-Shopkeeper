using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerAnimator : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        //set value of references using getcomponent
        anim = GetComponent<Animator>();
        agent = GetComponentInParent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", agent.velocity.magnitude / agent.speed);

        if (GetComponentInParent<CustomerMovement>().IsInspecting == true) {
            anim.SetTrigger("Inspecting");
        }
    }
}
