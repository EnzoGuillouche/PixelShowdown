using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravité : MonoBehaviour
{
    protected Vector2 velocity;
    private Rigidbody2D rb;

    public float gravityModifier = 1f;

    private float Move;

    public float speed;
    public float jump;

    bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(Move * speed, rb.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector2(rb.velocity.x, jump * 10));
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }
}
