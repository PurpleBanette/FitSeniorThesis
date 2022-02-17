using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    [Tooltip("How fast the player moves")] public float playerMovementSpeed = 10f;
    [Tooltip("How high the player jumps")] public float playerJumpForce = 10f;
    [Tooltip("The layer that is defined as the ground")] public LayerMask groundCheck;
    [Tooltip("Checks if the player is touching the ground")] public bool isGrounded;
    public Camera playerCamera;
    [Tooltip("The blink gameobject inside the player")] public GameObject blinkGameobject;
    /*[Tooltip("The speed at which the player dashes")]
    public float playerDash = 20f;*/
    [Tooltip("Determines whether the player is able to dash")] public bool isDashReady;
    [Tooltip("The destination that the player teleports to after blinking")] public Transform blinkDestination;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        isDashReady = true;
    }

    void Update()
    {
        PlayerMovementsXYZ();
    }

    void PlayerMovementsXYZ()
    {
        //Input
        float movementX = Input.GetAxisRaw("Horizontal") * playerMovementSpeed;
        float movementZ = Input.GetAxisRaw("Vertical") * playerMovementSpeed;
        //Movement
        Vector3 playerMovementPosition = transform.right * movementX + transform.forward * movementZ;
        Vector3 newPlayerMovementPosition = new Vector3(playerMovementPosition.x, playerRigidbody.velocity.y, playerMovementPosition.z);
        playerRigidbody.velocity = newPlayerMovementPosition;
        //Jumping
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, playerJumpForce, playerRigidbody.velocity.z);
        }
        //Grounded
        isGrounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), 0.4f, groundCheck);
        //Rotation locked to Camera
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, playerCamera.transform.localEulerAngles.y, transform.localEulerAngles.z);
        //Blink Direction
        Vector3 blinkDirection = new Vector3(movementX, 0f, movementZ);
        float blinkTargetAngle = Mathf.Atan2(blinkDirection.x, blinkDirection.z) * Mathf.Rad2Deg;
        blinkGameobject.transform.rotation = Quaternion.Euler(0f, blinkTargetAngle, 0f);
        //Dash Mechanic
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(isDashReady == true)
            {
                playerRigidbody.transform.position = blinkDestination.transform.position;
                StartCoroutine(BlinkCooldown());
                isDashReady = false;
            }
        }
        
    }

    IEnumerator BlinkCooldown()
    {
        yield return new WaitForSeconds(1);
        isDashReady = true;
    }
}
