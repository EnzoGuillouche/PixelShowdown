using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GutsActions : MonoBehaviour
{
    #region Variables
    private Rigidbody2D rb;
    private UserInputs inputName;
    private Animator animator;
    private GameManager gameManager;
    private float moveInput;
    public float speed = 12f;
    public float airSpeed = 8f;
    public float jump = 15f;
    public float Djump = 20f;
    public float dashDistance = 50f;
    public float dashDelay = 0.25f;
    public float dashCooldown = 5f;
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
    public int maxHealth = 100;
    public int health;
    #endregion

    #region Actions Functions

    private void Move()
    {
        if (canMove){
            // get the character's movement
            moveInput = Input.GetKeyDown(inputName.currentInputs["-X"]) ? -1 : Input.GetKeyDown(inputName.currentInputs["+X"]) ? 1 : Input.GetAxis("Horizontal" + transform.parent.name[transform.parent.name.Length - 1]);
        
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
    private void Jump() 
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
    private void Crouch()
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
    private IEnumerator Dash()
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
    private void Attack(){
        float verticalInput = Input.GetKeyDown(inputName.currentInputs["-Y"]) ? -1 : Input.GetKeyDown(inputName.currentInputs["+Y"]) ? 1 : Input.GetAxis("Vertical" + transform.parent.name[transform.parent.name.Length - 1]);
        // apply the attacks conditions and actions
        if (Input.GetKeyDown(inputName.currentInputs["Attack"]) && canMove && grounded && !isCrouching){
            animator.SetTrigger("attack");
            if (verticalInput > 0){ // up tilt
                rb.velocity = new Vector2(0, rb.velocity.y);
                animator.SetTrigger("attack3");
            }
            else if (rb.velocity.x != 0){ // f tilt
                rb.velocity = new Vector2(0, rb.velocity.y);
                animator.SetTrigger("attack2");
            }
            else { // jab
                animator.SetTrigger("attack1");
            }
        }
    }
    private void Special(){
        // apply the special attacks conditions and actions
        if (Input.GetKeyDown(inputName.currentInputs["SpeAttack"]) && canMove && grounded && !isCrouching){
            if (rb.velocity.x != 0 && spe2Cooldown >= 5f){ // side b
                animator.SetTrigger("attack");
                spe2Cooldown = 0f;
                rb.velocity = new Vector2(0, rb.velocity.y);
                animator.SetTrigger("spe2");
            }
            else if (spe1Cooldown >= 5f){ // neutral b
                animator.SetTrigger("attack");
                spe1Cooldown = 0f;
                rb.velocity = new Vector2(0, rb.velocity.y);
                animator.SetTrigger("spe1");
            }
        }
        // cooldowns
        else {
            if (spe1Cooldown < 5f)
                spe1Cooldown += Time.deltaTime;
            if (spe2Cooldown < 5f)
                spe2Cooldown += Time.deltaTime;
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
    private IEnumerator Death(){
        if (deathDelay > 0){
            deathDelay -= Time.deltaTime;
            yield return null;
        }
        else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
    #endregion

    #region System Functions
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.gravityScale = 4;
        inputName = transform.parent.GetComponent<UserInputs>();
        health = maxHealth;
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
        if (transform.parent.name.EndsWith("2")){
            Flip();
            isFacingRight = !isFacingRight;
        }
        else {
        }
    }

    void Update()
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

            Attack();

            Special();
        }
    }
    #endregion
}