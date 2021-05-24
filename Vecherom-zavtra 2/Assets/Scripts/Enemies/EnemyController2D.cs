using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using System.Linq;

public class EnemyController2D : NetworkBehaviour
{
	public float moveSpeed = 5f;
	[SerializeField] private float m_JumpForce = 100f;                          // Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.

	public GameObject healthBar;
	[HideInInspector] public Transform targetPlayer;
	private Animator animator;
	[SerializeField] float aggroDistance = 30f;
	[SerializeField] float jumpCooldown = 2f;
	[SerializeField] bool canJump = true;
	float jumpTime = 0f;
	public bool canMove = true;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}
	void Update()
	{
		if (!canMove) return;
		TryMoveToPlayer();
		if (canJump)
		{
			TryJump();
		}
	}

	void LateUpdate()
	{
		TrySwitchTarget();

		if (targetPlayer != null)
		{
			if (targetPlayer.transform.position.x - transform.position.x > 0 && !m_FacingRight)
			{
				Flip();
			}
			else if (targetPlayer.transform.position.x - transform.position.x < 0 && m_FacingRight)
			{
				Flip();
			}
		}
		
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}

	private void TryMoveToPlayer()
	{
		if (targetPlayer != null && Vector3.Distance(targetPlayer.position, transform.position) <= aggroDistance)
		{
			var direction = targetPlayer.position.x - transform.position.x;
			var move = targetPlayer.position - transform.position;
			move.y = m_Rigidbody2D.velocity.y;
			move.Normalize();
			
			m_Rigidbody2D.MovePosition((Vector2)transform.position + ((Vector2) move * moveSpeed * Time.deltaTime));

			animator.SetFloat("Movement", Mathf.Abs(direction));
		}
	}

	private void TrySwitchTarget()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		if (players.Length > 0)
		{
			targetPlayer = players[0].transform;
			foreach (var player in players)
			{
				// If a player is nearer than the current target, switch to it
				if (Vector3.Distance(player.transform.position, transform.position) < Vector3.Distance(targetPlayer.position, transform.position))
					targetPlayer = player.transform;
			}
		}
	}

	private void TryJump()
	{
		jumpTime += Time.deltaTime;

		if (jumpTime >= jumpCooldown)
		{
			GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
			bullets = bullets.Where(x => Mathf.Abs(x.transform.position.x - transform.position.x) <= 15f).ToArray();

			if (bullets.Length > 0)
			{
				GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 100f), ForceMode2D.Impulse);
				jumpTime = 0f;
			}
		}
	}



	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		FlipHealthBar();
	}
	private void FlipHealthBar()
	{
		Vector3 theScale = healthBar.transform.localScale;
		theScale.x *= -1;
		healthBar.transform.localScale = theScale;
	}
}
