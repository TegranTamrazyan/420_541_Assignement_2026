using UnityEngine;

// Moves the player with a Rigidbody: 
// A/D or Left/Right arrows to ROTATE, W/S or Up/Down to move Forward/Backward, Space to jump.
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;      // units per second
    [SerializeField] private float sprintSpeed = 45f;   // units per second
    [SerializeField] private float turnSpeed = 150f;    // degrees per second
    [SerializeField] private float maxJumpForce = 7f;      // strength of the jump
    [SerializeField] private float doubleJumpForce = 4f; // strength of the double jump
    [SerializeField] private float maxJumpHoldTime = 3f;

    [Header("Ground Check Settings")]
    [SerializeField] private float groundDistance = 0.5f; // ray length
    [SerializeField] private LayerMask groundMask;        // ground layers

    private Rigidbody rb;               // the physics body we move
    private Vector3 moveDirection;      // worked out from input in Update()
    private float turnInput;            // horizontal input for rotation
    private float jumpButtonHeld = 0.0f;
    private bool isJumping = false;
    private bool isSprinting = false;   // true when holding down the sprint key
    private bool isGrounded;            // true when standing on the ground
    private bool jumpRequested = false; // set in Update(), used later
    private bool doubleJumpRequested = false; // set in TryDoubleJump()
    private bool hasDoubleJumped = false;     // one per jump
    private bool canDoubleJump = false;     // double jump activated from picking up jump-boost
    private bool doubleSpeedActivated = false; // double speed activated from picking up speed-boost


    public bool IsGrounded => isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        // 1. Ground Check
        isGrounded = Physics.Raycast(transform.position + transform.up*groundDistance/2, -transform.up, groundDistance, groundMask);

        // 2. Read Inputs
        turnInput = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right turn input
        float moveZ = Input.GetAxisRaw("Vertical");  // W/S or Up/Down forward/backward

        // Rotate the player transform in Update for responsive turning visuals
        transform.Rotate(0f, turnInput * turnSpeed * Time.deltaTime, 0f);

        // Calculate forward movement relative to current facing direction
        moveDirection = transform.forward * moveZ;

        // 3. Handle Jump Input
        if (Input.GetButtonDown("Jump") && isGrounded && !isJumping)
        {
            isJumping = true;

        }
        if (Input.GetButton("Jump") && isGrounded && isJumping)
        {
            jumpButtonHeld += Time.deltaTime;

            // Automatically jump after 3 seconds
            if (jumpButtonHeld >= maxJumpHoldTime)
            {
                jumpButtonHeld = maxJumpForce;
                jumpRequested = true;
                isJumping = false;
            }

        }
        if (Input.GetButtonUp("Jump") && isJumping)
        {
            if(jumpButtonHeld < 1.5f)
            {
                jumpButtonHeld = maxJumpForce / 2; // If the jump button is held for less than 1.5 seconds, the jump force is so minimal that the player will barely jump at all. so balancing it a little. 
            }
            else
            {
                jumpButtonHeld = maxJumpForce / (maxJumpHoldTime / jumpButtonHeld);
            }
            jumpRequested = true;
            isJumping = false;
        }
        // Back on the ground: the double jump is available again
        if (isGrounded)
        {
            hasDoubleJumped = false;
        }
    }

    private void FixedUpdate()
    {
        SprintActivation();

        MovePlayer();

        if (jumpRequested)
        {
            Jump(jumpButtonHeld);
            jumpRequested = false;
            jumpButtonHeld = 0.0f; // Reset for next jump
        }
        // NEW: the second jump, in mid-air
        if (doubleJumpRequested)
        {
            Jump(doubleJumpForce);
            doubleJumpRequested = false;
        }

    }

    private void MovePlayer()
    {
        if (doubleSpeedActivated)
        {
            // Calculate velocity based on current forward vector
            Vector3 targetVelocity = 2 * (moveDirection * (isSprinting ? sprintSpeed : moveSpeed));
            // Apply movement while preserving vertical velocity (gravity/jumping)
            rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
        }
        else
        {
            // Calculate velocity based on current forward vector
            Vector3 targetVelocity = moveDirection * (isSprinting ? sprintSpeed : moveSpeed);
            // Apply movement while preserving vertical velocity (gravity/jumping)
            rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
        }
    }

    private void Jump(float force)
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * force, ForceMode.VelocityChange);
    }

    public bool TryDoubleJump()
    {
        // Not allowed on the ground, or if it was already used
        if (isGrounded || hasDoubleJumped || !canDoubleJump) return false;

        doubleJumpRequested = true;
        hasDoubleJumped = true;
        return true;
    }

    private void SprintActivation()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
    }

    public void ActivateSpeedBoost()
    {
        doubleSpeedActivated = true;
    }
    public void DeactivateSpeedBoost()
    {
        doubleSpeedActivated = false;
    }
    public void ActivateJumpBoost()
    {
        canDoubleJump = true;
    }
    public void DeactivateJumpBoost()
    {
        canDoubleJump = false;
    }

}