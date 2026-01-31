using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 15f;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheckObj;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.8f, 0.2f);

    [Header("Game Feel Settings")]
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.2f;

    // Components
    private Rigidbody2D rb;
    private GameControls controls;
    private Animator animator;

    // Variables
    private float moveInput;
    private bool isGrounded;
    private bool isFacingRight = true;

    // Game Feel Counters
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new GameControls();
        animator = GetComponent<Animator>();

        controls.Gameplay.Jump.performed += context =>
                {
                    jumpBufferCounter = jumpBufferTime; // buffer the jump input
                };

        // Stop jumping when the jump button is released (for short jumps)
        controls.Gameplay.Jump.canceled += context =>
        {
            if (rb.linearVelocity.y > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }
        };
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {
        // Flip the player based on movement direction
        if (moveInput > 0f && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0f && isFacingRight)
        {
            Flip();
        }

        // Cayote Time
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Jump Buffer
        jumpBufferCounter -= Time.deltaTime;
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            Jump();

            jumpBufferCounter = 0f;
        }

        moveInput = controls.Gameplay.Move.ReadValue<float>();

        UpdateAnimaton();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapBox(groundCheckObj.position, groundCheckSize, 0f, groundLayer);
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 scaler = transform.localScale;

        scaler.x *= -1;

        transform.localScale = scaler;
    }

    private void UpdateAnimaton()
    {
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        animator.SetBool("IsGrounded", isGrounded);

        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    //For debugging
    private void OnDrawGizmos()
    {
        if (groundCheckObj != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheckObj.position, groundCheckSize);
        }
    }
}