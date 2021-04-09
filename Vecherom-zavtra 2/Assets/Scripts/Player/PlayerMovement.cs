using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Transform background;
    public Rigidbody2D rb;
    public Animator animator;

    public float runSpeed = 30f;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

	void Awake()
	{
        
	}

	// Update is called once per frame
	void Update()
    {
        animator.SetFloat("playerSpeed", Mathf.Abs(horizontalMove));
        background.localPosition = new Vector3(background.localPosition.x + (rb.velocity.x * 0.0001f), 0, 10f);
    }

	void FixedUpdate()
	{
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
	}

    public void Move(InputAction.CallbackContext context)
	{
        horizontalMove = context.ReadValue<float>() * runSpeed;
	}

    public void Jump(InputAction.CallbackContext context)
	{
        if (context.performed)
		{
            jump = context.ReadValue<float>() >= 0.2f;
            animator.SetBool("isJumping", true);
        }
    }

    public void Crouch(InputAction.CallbackContext context)
	{
        crouch = context.ReadValue<float>() >= 0.4f;
    }

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
    }
}
