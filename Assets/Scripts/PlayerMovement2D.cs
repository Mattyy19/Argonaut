using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Code used from here: https://qookie.games/2d-player-movement/ 
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float jumpForce = 13f;
    public float sprintSpeed = 2f;

    [Header("Jetpack")]
    public bool acquired = false;
    public float rechargeRate = 1f;
    public float burnRate = 1f;
    public float jetpackForce = 15f;
    public float capacity = 3f;

    [Header("Blaster")]
    public Transform firePoint;

    [Header("Health Component")]
    public Health playerHealth;

    private bool isGrounded;
    private bool onIce;
    private float currFuel;
    private Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        currFuel = capacity;
    }

    void Update()
    {
        // Handle horizontal movement
        float moveInput = Input.GetAxis("Horizontal");
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        float currSpeed = isSprinting ? moveSpeed * sprintSpeed : moveSpeed;
        float targetSpeed = moveInput * currSpeed;

        float accel = onIce ? 10f : 200f; // lower 30 to make more slip
        float horizontalVelocity = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, accel * Time.deltaTime);

        rb.linearVelocity = new Vector2(horizontalVelocity, rb.linearVelocity.y);

        // Handles jetpack movement, must have enough fuel to fly
        bool jetpackPressed = Input.GetKey(KeyCode.Q);
        if (jetpackPressed && currFuel > 0f && acquired)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jetpackForce);
            currFuel -= burnRate * Time.deltaTime;
            currFuel = Mathf.Max(currFuel, 0f);
        }
        // Refills jetpack fuel
        if (isGrounded && currFuel < capacity)
        {
            currFuel += rechargeRate * Time.deltaTime;
            currFuel = Mathf.Min(currFuel, capacity);
        }

        // Flip sprite based on movement direction
        if (spriteRenderer != null && moveInput != 0)
        {
            bool flip = moveInput < 0;
            spriteRenderer.flipX = flip;

            // Flip the firePoint position and facing direction
            if (firePoint != null)
            {
                Vector3 localPos = firePoint.localPosition;
                localPos.x = Mathf.Abs(localPos.x) * (flip ? -1 : 1);
                firePoint.localPosition = localPos;

                Vector3 localRot = firePoint.localEulerAngles;
                localRot.y = flip ? 180 : 0;
                firePoint.localEulerAngles = localRot;
            }
        }

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        // Check if the player is on Ice
        else if (collision.gameObject.CompareTag("Ice"))
        {
            isGrounded = true;
            onIce = true;
        }
        else if (collision.gameObject.CompareTag("Slime"))
        {
            float bounceMultiplier = collision.gameObject.GetComponent<SlimePlatform>().bounceMultiplier;
            float bounce = jumpForce * bounceMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounce);
        }
        // Check if the player touching a hazard
        else if (collision.gameObject.CompareTag("Hazard"))
        {
            playerHealth.TakeDamage(100);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the player is no longer on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        // Check if the player is no longer on ice
        if (collision.gameObject.CompareTag("Ice"))
        {
            isGrounded = false;
            onIce = false;
        }
    }
}
