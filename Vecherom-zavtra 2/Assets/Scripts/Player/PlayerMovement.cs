using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    //public InputMaster inputs;

    public float runSpeed = 30f;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

	void Awake()
	{
        /*inputs = new InputMaster();
        inputs.Player.Movement.performed += ctx => Move(ctx.ReadValue<int>());
        inputs.Player.Jump.performed += _ => Jump();
        inputs.Player.Crouch.performed += _ => crouch = !crouch;*/
	}

	// Update is called once per frame
	void Update()
    {
        animator.SetFloat("playerSpeed", Mathf.Abs(horizontalMove));

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
        jump = context.ReadValue<float>() >= 0.2f;
    }

    public void Crouch(InputAction.CallbackContext context)
	{
        crouch = context.ReadValue<float>() >= 0.4f;
    }
}
