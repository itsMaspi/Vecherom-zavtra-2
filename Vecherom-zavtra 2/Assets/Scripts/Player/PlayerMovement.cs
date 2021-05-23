using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    public CharacterController2D controller;
    public Transform background;
    public Rigidbody2D rb;
    public Animator animator;

    public float runSpeed = 30f;

    public float horizontalMove = 0f;
    bool jump = false;
    bool dash = false;

    DashState dashState = DashState.Ready;
    float dashTimer;
    public float maxDash = 10f;

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;
        animator.SetFloat("playerSpeed", Mathf.Abs(horizontalMove));
        if (dashState == DashState.Cooldown)
		{
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                dashTimer = 0;
                dashState = DashState.Ready;
            }
        }
        //background.localPosition = new Vector3(background.localPosition.x + (rb.velocity.x * 0.0001f), 0, 10f);
    }

    void FixedUpdate()
    {
        //if (!isLocalPlayer) return;
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash);
        jump = false;
        dash = false;
    }

    public void OnMovement(InputValue value) // InputAction.CallbackContext context
    {
        //if (!isLocalPlayer) return;
        //horizontalMove = context.ReadValue<float>() * runSpeed;
        if (PauseManager.pauseState == PauseState.Paused) return;
        if (GetComponent<PlayerController>().isChatting) return;
        horizontalMove = value.Get<float>() * runSpeed;
    }

    public void OnJump(InputValue value)
    {
        //if (!isLocalPlayer) return;
        /*if (context.performed)
        {
            jump = context.ReadValue<float>() >= 0.2f;
            animator.SetBool("isJumping", true);
        }*/
        if (PauseManager.pauseState == PauseState.Paused) return;
        if (GetComponent<PlayerController>().isChatting) return;
        if (value.isPressed)
		{
            jump = true;
            animator.SetBool("isJumping", true);
        }
    }

    public void OnDash(InputValue value)
    {
        if (PauseManager.pauseState == PauseState.Paused) return;
        if (GetComponent<PlayerController>().isChatting) return;
        if (dashState == DashState.Ready)
		{
            dash = true;
            dashTimer = maxDash;
            dashState = DashState.Cooldown;
        }

    }

    public void OnLanding()
    {
        //if (!isLocalPlayer) return;
        animator.SetBool("isJumping", false);
    }
}

public enum DashState
{
    Ready,
    Cooldown
}