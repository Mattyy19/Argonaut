using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Code used from here: https://qookie.games/2d-player-movement/ 
//TODO: Add sprint and crouch
public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float jumpForce = 13f;
    public Health playerHealth;
    private bool isGrounded;
    private Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        // Handle horizontal movement
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Flip sprite based on movement direction
        if (spriteRenderer != null && moveInput != 0)
        {
            spriteRenderer.flipX = moveInput < 0; // true when moving left
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
        // Check if the player touching a hazard
        if (collision.gameObject.CompareTag("Hazard"))
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
    }
}
