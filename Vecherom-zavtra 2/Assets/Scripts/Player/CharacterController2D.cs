using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class CharacterController2D : NetworkBehaviour
{
	[SerializeField] private int m_MaxJumps = 1;                                 // Number of jumps avaliable
	private int m_Jumps = 1;
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	public GameObject interactionIcon;
	public GameObject nickname;

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	public override void OnStartLocalPlayer()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		m_Jumps = m_MaxJumps;

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}
	/*private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}*/

	private void FixedUpdate()
	{
		if (!isLocalPlayer) return;
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
				m_Jumps = m_MaxJumps;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}


	public void Move(float move, bool jump, bool dash)
	{
		if (!isLocalPlayer) return;
		
		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (jump && m_Jumps-- > 0)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce), ForceMode2D.Impulse);
		}
		if (dash)
		{
			//m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, new Vector3(1500f * transform.localScale.normalized.x, m_Rigidbody2D.velocity.y), ref m_Velocity, 0.25f);
			//m_Rigidbody2D.AddForce(new Vector2(3000f * transform.localScale.normalized.x, 0f));
			m_Rigidbody2D.AddForce(Vector2.right * 120f * transform.localScale.normalized.x, ForceMode2D.Impulse);
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
		FlipInteractionIcon();
		FlipNickname();
	}
	private void FlipInteractionIcon()
	{
		Vector3 thePosition = interactionIcon.transform.localPosition;
		thePosition.x *= -1;
		interactionIcon.transform.localPosition = thePosition;
	}
	private void FlipNickname()
	{
		Vector3 theScale = nickname.transform.localScale;
		theScale.x *= -1;
		nickname.transform.localScale = theScale;
	}
}