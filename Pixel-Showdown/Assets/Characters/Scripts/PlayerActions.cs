using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerActions : MonoBehaviour
{
    #region Variables
    protected Rigidbody2D rb;
    protected UserInputs inputName;
    protected Animator animator;
    protected GameManager gameManager;
    protected float moveInput;
    public float speed;
    public float airSpeed;
    public float jump;
    public float Djump;
    public float dashDistance;
    public float dashDelay;
    public float dashCooldown;
    public float spe1Cooldown = 5f;
    public float spe2Cooldown = 5f;
    public float deathDelay = 3f;
    public bool grounded;
    public bool canMove;
    public bool cannotGoLeft = false;
    public bool cannotGoRight = false;
    public bool isCrouching = false;
    public bool hasJumpedTwice;
    public bool isFacingRight = true;
    public bool isAlive = true;
    public int maxHealth;
    public int health;
    #endregion

    #region Actions Functions

    protected void Move()
    {
        if (canMove){
            // get the character's movement
            moveInput = Input.GetKeyDown(inputName.currentInputs["-X"]) ? -1 : Input.GetKeyDown(inputName.currentInputs["+X"]) ? 1 : Input.GetAxis("Horizontal" + transform.parent.name[transform.parent.name.Length - 1]);
        
            // animations' conditions
            animator.SetBool("isMoving", moveInput != 0);

            // flip the sprite, depending on the orientation
            if (moveInput > 0 && !isFacingRight && grounded && !isCrouching && !animator.GetBool("isDashing"))
            {
                Flip();
            }
            
            else if (moveInput < 0 && isFacingRight && grounded && !isCrouching && !animator.GetBool("isDashing"))
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
    protected void Jump() 
    {
        // apply the jump action (double jump management)
        if (Input.GetKeyDown(inputName.currentInputs["Jump"]) && !hasJumpedTwice && canMove) {
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
    protected void Crouch()
    {
        // apply the crouch conditions
        if (Input.GetKey(inputName.currentInputs["Crouch"]) && grounded && canMove)
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
    protected IEnumerator Dash()
    {
        // apply the velocity and the anim depending on dash conditions
        if (Input.GetKeyDown(inputName.currentInputs["Dash"]) && canMove && dashCooldown >= 5f){
            dashCooldown = 0f;
            animator.SetBool("isDashing", true);
            canMove = false;
            rb.gravityScale = 0;
            while (dashDelay > 0){
                rb.velocity = new Vector2(isFacingRight ? dashDistance : -dashDistance, 0);
                dashDelay -= Time.deltaTime;
                yield return null;
            }
            animator.SetBool("isDashing", false);
            dashDelay = 0.25f;
            canMove = true;
            rb.gravityScale = 4;
        }
        // dash cooldown
        else if (dashCooldown < 5f) {
            dashCooldown += Time.deltaTime;
            yield return null;
        }
    }
    public void Hit(int damage)
    {
        if (isAlive && !isCrouching){
            health -= damage;
        }

        if (health <= 0){
            health = 0;
            if (transform.parent.name.EndsWith("1")){
                gameManager.lives1--;
            }
            else {
                gameManager.lives2--;
            }
            animator.SetBool("isAlive", false);
        }
    }
    protected IEnumerator Death()
    {
        if (deathDelay > 0){
            deathDelay -= Time.deltaTime;
            yield return null;
        }
        else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    protected void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = isFacingRight ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
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
    #endregion

    #region System Functions
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inputName = transform.parent.GetComponent<UserInputs>();
        health = maxHealth;
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
        if (transform.parent.name.EndsWith("2")){
            Flip();
        }
    }

    protected virtual void Update()
    {
        isAlive = animator.GetBool("isAlive");
        if (!isAlive){
            Debug.Log(gameObject.name + transform.parent.name[transform.parent.name.Length - 1] + " is no longer alive");
            StartCoroutine(Death());
        } else {
            canMove = animator.GetBool("canMove");

            Crouch();

            Move();

            Jump();

            StartCoroutine(Dash());

        }
    }
    #endregion
}