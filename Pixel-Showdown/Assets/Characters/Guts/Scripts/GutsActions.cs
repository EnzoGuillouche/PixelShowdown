using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class GutsActions : MonoBehaviour
{
    // variables
    Rigidbody2D rb;

    private float moveInput;
    public float speed = 12f;
    public float airSpeed = 8f;
    public float jump = 15f;
    public float Djump = 20f;
    public float dashDistance = 50f;
    public float delay = 1f;
    public bool grounded;
    public bool canMove = true;
    public bool cannotGoLeft = false;
    public bool cannotGoRight = false;
    public bool isCrouching = false;
    public bool hasJumpedTwice;
    public bool isFacingRight = true;

    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.gravityScale = 4;
    }

    void Update()
    {
            Crouch();

            Move();

            Jump();

           StartCoroutine(Dash());
    }

    private void Move()
    {
        if (canMove){
            // get the character's movement
            moveInput = Input.GetKeyDown(UserInputs1.currentInputs["-X"]) ? -1 : Input.GetKeyDown(UserInputs1.currentInputs["+X"]) ? 1 : Input.GetAxis("Horizontal");
        
            // animations' conditions
            if (moveInput != 0)
            {
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            // flip the sprite, depending on the orientation
            if (moveInput > 0 && !isFacingRight && grounded && !isCrouching)
            {
                Flip();
            }
            
            else if (moveInput < 0 && isFacingRight && grounded && !isCrouching)
            {
                Flip();
            }

            // apply the movement's speed (wall collisions management)
            if (!isCrouching && !((cannotGoLeft && moveInput < 0) || (cannotGoRight && moveInput > 0)))
            {
                rb.velocity = new Vector2(grounded ? moveInput * speed : moveInput * airSpeed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }

    private void Jump() 
    {
        // apply the jump action (double jump management)
        if (Input.GetKeyDown(UserInputs1.currentInputs["Jump"]) && !hasJumpedTwice && canMove)
        {
            if (!grounded)
            {
                hasJumpedTwice = true;
                if (rb.velocity.x < 0 && isFacingRight){
                    animator.SetTrigger("reverseDoubleJumped");
                }
                else if (rb.velocity.x > 0 && !isFacingRight){
                    animator.SetTrigger("reverseDoubleJumped");
                }
                else{
                    animator.SetTrigger("doubleJumped");
                }
            }
            rb.velocity = new Vector2(rb.velocity.x, grounded ? jump : Djump);
            
        }
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    private void Crouch()
    {
        // apply the crouch conditions
        if (Input.GetKey(UserInputs1.currentInputs["Crouch"]) && grounded && canMove)
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

    private IEnumerator Dash()
    {
        // apply the velocity and the anim depending on dash conditions
        if (Input.GetKeyDown(UserInputs1.currentInputs["Dash"]) && canMove){
            animator.SetBool("isDashing", true);
            canMove = false;
            rb.gravityScale = 0;
            while (delay > 0){
                rb.velocity = new Vector2(isFacingRight ? dashDistance : -dashDistance, 0);
                delay -= Time.deltaTime;
                yield return null;
            }
            animator.SetBool("isDashing", false);
            delay = 0.25f;
            canMove = true;
            rb.gravityScale = 4;
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
            animator.SetBool("isGrounded", true);
            hasJumpedTwice = false;
        }
        if (other.gameObject.CompareTag("LeftWall"))
        {
            cannotGoLeft = true;
            // hasJumpedTwice = false; // for wall jump
        }
        if (other.gameObject.CompareTag("RightWall"))
        {
            cannotGoRight = true;
            // hasJumpedTwice = false; // for wall jump
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = false;
            animator.SetBool("isGrounded", false);
        }
        if (other.gameObject.CompareTag("LeftWall"))
        {
            cannotGoLeft = false;
        }
        if (other.gameObject.CompareTag("RightWall"))
        {
            cannotGoRight = false;
        }
    }
}
