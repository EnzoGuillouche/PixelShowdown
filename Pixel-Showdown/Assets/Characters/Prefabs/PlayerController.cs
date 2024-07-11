using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float dashDistance = 5f;
    public float dashDuration = 0.2f;
    public float crouchSpeedMultiplier = 0.5f;

    private bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // R�cup�ration des inputs � chaque frame
        float moveInput = Input.GetAxis("Horizontal");

        // Gestion du mouvement horizontal
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // V�rification si le joueur est au sol
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        // Gestion du saut
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Gestion du dash
        if (Input.GetButtonDown("Dash"))
        {
            StartCoroutine(Dash());
        }

        // Autres actions comme l'attaque, etc.
        // Exemple : if (Input.GetButtonDown("Att1")) { ... }
    }

    // Coroutine pour g�rer le dash
    private IEnumerator Dash()
    {
        // D�sactiver la gravit� pendant le dash (optionnel)
        rb.gravityScale = 0f;

        // Calcul de la direction du dash
        Vector2 dashDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized * dashDistance;

        // Appliquer le dash pendant la dur�e d�finie
        float dashTimer = 0f;
        while (dashTimer < dashDuration)
        {
            rb.velocity = dashDirection / dashDuration;
            dashTimer += Time.deltaTime;
            yield return null;
        }

        // R�activer la gravit� apr�s le dash (optionnel)
        rb.gravityScale = 1f;
    }
}
