using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public bool playerCanControlCharacter = true;

    //player mouse senstivity
    public float maxSens_x = 450;
    public float maxSens_y = 2;

    private float gravity = 9.81f;
    private float vSpeed = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCanControlCharacter) {
            float horizontal = Input.GetAxisRaw("Horizontal"); //-1 == a, 1 == d
            float vertical = Input.GetAxisRaw("Vertical"); //-1 == s , 1 == w
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                Vector3 vec = moveDir.normalized * speed;

                if (controller.isGrounded) //applying gravity
                {
                    vSpeed = 0;
                }
                else {
                    vSpeed -= gravity * Time.deltaTime;
                    vec.y = vSpeed;
                    
                }
                controller.Move(vec * Time.deltaTime);
            }
            else {
                controller.SimpleMove(Vector3.zero);
            }
        }

    }
}
