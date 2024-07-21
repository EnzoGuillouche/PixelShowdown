using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    // variables
    Rigidbody2D rb;

    private float moveInput;
    public float speed = 10f;
    public float jump = 10f;
    public bool grounded;
    public bool isCrouching = false;
    public bool hasJumpedTwice;
    public bool isFacingRight = true;

    Animator animator;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Crouch();

        Move();

        Jump();
    }

    private void Move()
    {
        // get the character's movement
        moveInput = Input.GetKeyDown(UserInputs.currentInputs["-X"]) ? -1 : Input.GetKeyDown(UserInputs.currentInputs["+X"]) ? 1 : Input.GetAxis("Horizontal");

        // animations' conditions
        if (moveInput != 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }

        // apply the movement's speed
        if (!isCrouching)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void Jump() 
    {
        // apply the jump action (double jump management)
        if (Input.GetKeyDown(UserInputs.currentInputs["Jump"]) && !hasJumpedTwice)
        {
            if (!grounded)
            {
                hasJumpedTwice = true;
            }
            rb.velocity = new Vector2(rb.velocity.x, jump);

        }
    }

    private void Crouch()
    {
        // apply the crouch conditions
        if (Input.GetKey(UserInputs.currentInputs["Crouch"]) && grounded)
        {
            animator.SetBool("isCrouching", true);
            isCrouching = true;
        }
        else 
        {
            animator.SetBool("isCrouching", false);
            isCrouching = false;
        }
    }

    private void Flip()
    {
        transform.localScale *= new Vector2(-1, 1);
        isFacingRight = !isFacingRight;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            hasJumpedTwice = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }
}
