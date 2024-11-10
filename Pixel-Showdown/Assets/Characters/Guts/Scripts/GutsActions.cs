using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GutsActions : PlayerActions
{

    #region Actions Functions

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
    #endregion

    #region System Functions
    protected override void Start()
    {
        base.Start();
        // set local variables
        rb.gravityScale = 4;
        speed = 12f;
        airSpeed = 8f;
        jump = 15f;
        Djump = 20f;
        dashCooldown = 5f;
        dashDistance = 50f;
        dashDelay = 0.25f;
        spe1Cooldown = 5f;
        spe2Cooldown = 5f;
        deathDelay = 3f;
        maxHealth = 100;
        health = maxHealth;
    }

    protected override void Update()
    {
        base.Update();
        if (isAlive)
        {
            Attack();
            Special();
        }
    }


    #endregion
}