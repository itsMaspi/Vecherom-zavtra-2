using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Transform background;
    public Rigidbody2D rb;
    //public InputMaster inputs;

    public float runSpeed = 30f;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    private Vector2 boxSize = new Vector2(0.1f, 1f);

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
        jump = context.ReadValue<float>() >= 0.2f;
    }

    public void Crouch(InputAction.CallbackContext context)
	{
        crouch = context.ReadValue<float>() >= 0.4f;
    }

    public void CheckInteraction(InputAction.CallbackContext context)
	{
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, boxSize, 0, Vector2.zero);

		if (hits.Length > 0)
		{
			foreach (var rc in hits)
			{
                if (rc.transform.GetComponent<Interactable>())
				{
                    rc.transform.GetComponent<Interactable>().Interact();
                    return;
                }
			}
		}
	}
}
