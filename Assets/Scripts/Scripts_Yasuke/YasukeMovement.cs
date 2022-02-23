﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YasukeMovement : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    private Vector3 moveDirection;
    private Vector3 velocity;


    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistanced;
    [SerializeField] private LayerMask groundedMask;
    [SerializeField] private float gravity;

    [SerializeField] private float jumpHeight;


    //Referances
    private CharacterController controller;
    private Animator anim;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        Move();

    }
    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistanced, groundedMask);

        if (isGrounded)
        {

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;

            }

            float moveZ = Input.GetAxis("Vertical");
            moveDirection = new Vector3(0, 0, moveZ);



            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                Run();
            }
            else if (moveDirection == Vector3.zero)
            {
                Idle();
            }
            moveDirection *= walkSpeed;

            if(Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        
        controller.Move(moveDirection * Time.deltaTime);

        velocity. y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);


    }
    private void Idle()
    {
        anim.SetFloat("Speed", 0);
    }
    private void Walk()
    {
        moveDirection *= walkSpeed;
        anim.SetFloat("Speed", 0.5f);
    }
    private void Run()
    {
        moveDirection *= walkSpeed;
        anim.SetFloat("Speed", 1f);
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }
}
