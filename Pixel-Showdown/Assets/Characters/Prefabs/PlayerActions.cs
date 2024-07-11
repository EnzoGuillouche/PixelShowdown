using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
  
    protected Vector2 velocity;
    private Rigidbody2D rb;

    private float Move;
    public float speed;
    public float jump;
    bool grounded;
    bool hasJumpedTwice;

    // animations' variables

    Animator animator;

    private bool _isMoving = false;
    public bool isMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool("isMoving", value);
        }
    }

    public bool _isFacingRight = true;
    public bool isFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }

            _isFacingRight = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // get the character's movement
        Move = Input.GetAxisRaw("Horizontal");

        // animations' conditions
        isMoving = Move != 0;
        if (Move > 0 && !isFacingRight) 
        { 
            isFacingRight = true;
        } else if (Move < 0 && isFacingRight)
        {
            isFacingRight = false;
        }

        // apply the movement's speed
        rb.velocity = new Vector2(Move * speed, rb.velocity.y);

        // apply the jump action (double jump management)
        if (Input.GetButtonDown("Jump") && !hasJumpedTwice)
        {
            if (!grounded)
            { 
                hasJumpedTwice = true;
            }
            rb.velocity = new Vector2(rb.velocity.x, jump);

        }

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
