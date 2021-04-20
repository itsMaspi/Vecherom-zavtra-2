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
    bool crouch = false;

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;
        animator.SetFloat("playerSpeed", Mathf.Abs(horizontalMove));
        //background.localPosition = new Vector3(background.localPosition.x + (rb.velocity.x * 0.0001f), 0, 10f);
    }

    void FixedUpdate()
    {
        //if (!isLocalPlayer) return;
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    public void OnMovement(InputValue value) // InputAction.CallbackContext context
    {
        //if (!isLocalPlayer) return;
        //horizontalMove = context.ReadValue<float>() * runSpeed;
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
        jump = true;
        animator.SetBool("isJumping", true);
    }

    public void OnCrouch(InputValue value)
    {
        //if (!isLocalPlayer) return;
        //crouch = context.ReadValue<float>() >= 0.4f;
        crouch = value.isPressed;
    }

    public void OnLanding()
    {
        //if (!isLocalPlayer) return;
        animator.SetBool("isJumping", false);
    }
}