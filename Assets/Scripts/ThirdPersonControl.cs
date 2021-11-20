using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThirdPersonControl : MonoBehaviour
{
    CapsuleCollider playerCapCollider;

    public static ThirdPersonControl instance;

    Animator playerAnimator;
    //Integers for animation hashes
    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;
    int landedGround;

    [Tooltip("Drag the GameObject's character controller here")]
    public CharacterController playerController;

    [Tooltip("Drag the active camera here")]
    public Transform playerCamera;

    [Tooltip("How fast the character walks")]
    public float walkSpeed = 10f;
    [Tooltip("How fast the character runs")]
    public float runSpeed = 20f;
    [Tooltip("How high the character jumps")]
    public float jumpHeight = 10f;
    [Tooltip("How fast the character turns horizontally")]
    public float turnSmoothTime = 0.1f;
    [SerializeField] private float turnSmoothVelocity;
    [Tooltip("How heavy the character is")]
    public float playerGravity = -80f;
    [Tooltip("How fast the character falls")]
    Vector3 fallVelocity;
    [Tooltip("The radius of the ground sensor")]
    public float groundDistance = 0.5f;
    [Tooltip("the mask that the ground sensor detects")]
    public LayerMask ground;
    [Tooltip("If the player is grounded or not")]
    private bool isGrounded = true;
    [Tooltip("Drag the player's sensor GameObject here")]
    private Transform groundSensor;

    [Tooltip("Max health of the player")]
    public int maxHealth = 100;
    [Tooltip("The current health of the player")]
    public int currentHealth;
    //[Tooltip("References the healthbar script")]
    //public HealthBar healthBar;
    

    private Vector3 playerMovement;


    void Awake()
    {
        
        playerController = GetComponent<CharacterController>();
        playerCapCollider = GetComponent<CapsuleCollider>();
        playerAnimator = GetComponent<Animator>();
        //groundSensor = transform.GetChild(0);
    }

    void Start()
    {
        Cursor.visible = false; //Invisible cursor
        Cursor.lockState = CursorLockMode.Locked; //Locks cursor to center
        //Animation hashes for better performance
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        landedGround = Animator.StringToHash("landed");

        playerMovement = Vector3.zero;



        //jPad = GameObject.FindGameObjectsWithTag("Jumppad");

        //PlayerHealth();
    }

    void Update()
    {
        PlayerMovement();
        //PlayerMovement2();
        /*void TakeDamage(int damage)
        {
            currentHealth -= damage;

            healthBar.SetHealth(currentHealth);
        }*/

    }

    void FixedUpdate()
    {
        PlayerAnimations();
    }

   
    
    void PlayerMovement()
    {
        //isGrounded = Physics.CheckSphere(groundSensor.position, groundDistance, ground, QueryTriggerInteraction.Ignore);
        float playerMovementX = Input.GetAxisRaw("Horizontal"); //Raw removes the smoothness of the transition of speed
        float playerMovementZ = Input.GetAxisRaw("Vertical");
        Vector3 playerMoveDirection = new Vector3(playerMovementX, 0f, playerMovementZ).normalized; //Normalized makes it so you don't go faster if pressing 2 directions
        fallVelocity.y += playerGravity * Time.deltaTime; //Gravity affects the velocity of the Y axis
        playerController.Move(fallVelocity * Time.deltaTime); //Player's Y axis responds to the gravity
        if (isGrounded && fallVelocity.y < 0) //If grounded and velocity is less than 0 (player is landing)
        {
            fallVelocity.y = -20f; //Set fall velocity to -20f (cleaner transition instead of player freezing in the air momentarily)
        }
        if (playerMoveDirection.magnitude >= 0.1) //Returns the length of the movement vector
        {
            float playerTargetAngle = Mathf.Atan2(playerMoveDirection.x, playerMoveDirection.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y; //Atan2 is a math function that returns the angle between the x axis and the vector starting at 0 and terminating at angle x,y
            float turnAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, playerTargetAngle, ref turnSmoothVelocity, turnSmoothTime); //turnSmoothVelocity stores the data
            transform.rotation = Quaternion.Euler(0f, turnAngle, 0f); //rotation

            Vector3 moveDir = Quaternion.Euler(0f, playerTargetAngle, 0f) * Vector3.forward; //Moving forward follows the forward direction of the player
            playerController.Move(moveDir.normalized * walkSpeed * Time.deltaTime); //Player's movement with walk speed
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) //If player presses Jump while grounded
        {
            fallVelocity.y = Mathf.Sqrt(jumpHeight * -2f * playerGravity); //Y velocity is equal to the jump hight (jumping), then is affected by gravity
        }
        bool playerRunning = Input.GetKey(KeyCode.LeftShift); //Shift Button
        if (playerRunning && playerMoveDirection.magnitude >= 1) //If shift is held and player is moving
        {
            float playerTargetAngle = Mathf.Atan2(playerMoveDirection.x, playerMoveDirection.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y; //Atan2 is a math function that returns the angle between the x axis and the vector starting at 0 and terminating at angle x,y
            float turnAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, playerTargetAngle, ref turnSmoothVelocity, turnSmoothTime); //turnSmoothVelocity stores the data
            transform.rotation = Quaternion.Euler(0f, turnAngle, 0f); //rotation

            Vector3 moveDir = Quaternion.Euler(0f, playerTargetAngle, 0f) * Vector3.forward;
            playerController.Move(moveDir.normalized * runSpeed * Time.deltaTime); //Player's movement with run speed
        }


    }

    void PlayerAnimations()
    {
        bool walkPressed = Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"); //Checks for movement keys
        bool runPressed = Input.GetKey("left shift"); //Checks for shift key
        bool jumpPressed = Input.GetKey("space");

        bool walkCheck = playerAnimator.GetBool(isWalkingHash); //Checks for the walking bool
        bool runCheck = playerAnimator.GetBool(isRunningHash); //Checks for the running bool

        bool landed = playerAnimator.GetBool(landedGround);
       

        //Walking
        if (!walkCheck && walkPressed) //If player is not walking and WASD is pressed
        {
            playerAnimator.SetBool(isWalkingHash, true);
        }
        if (walkCheck && !walkPressed) //If player is walking and WASD is not pressed
        {
            playerAnimator.SetBool(isWalkingHash, false);
        }
        //Running
        if (!runCheck && (walkPressed && runPressed)) //If player is not running and both keys are pressed
        {
            playerAnimator.SetBool(isRunningHash, true);
        }
        if (runCheck && (!walkPressed || !runPressed)) //If player is running and not pressing either key
        {
            playerAnimator.SetBool(isRunningHash, false);
        }
        //Jumping
        if (jumpPressed && isGrounded && landed)
        {
            playerAnimator.SetTrigger(isJumpingHash);
        }
        //Landed
        if (isGrounded)
        {
            playerAnimator.SetBool(landedGround, true);
        }
        if (!isGrounded)
        {
            playerAnimator.SetBool(landedGround, false);
        }
        /*
        if (healthtracker.instance.health <= 0)
        {
            playerAnimator.SetTrigger("isDead");
            Destroy(this.gameObject, 5f);
        }
        */
    }


}
