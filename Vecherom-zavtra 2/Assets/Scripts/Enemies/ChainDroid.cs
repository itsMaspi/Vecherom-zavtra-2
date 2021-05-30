using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChainDroid : NetworkBehaviour, IEnemy
{
	[Header("Stats")]
	public int maxHealth;
	[SerializeField] float attackDistance = 5f;
	[SerializeField] float attackCooldown = 1f;
	float attackTime = 0f;
	[SerializeField] private LayerMask whatCanDamage;

	[Space]
	[Header("Other")]

	[SyncVar(hook = nameof(OnChangedHealth))]
	[HideInInspector] public int currentHealth;

	[HideInInspector] public Animator animator;
	public Slider healthBar;

	private CharacterStats characterStats;
	[HideInInspector] public EnemyController2D controller;

	public void Awake()
	{
		characterStats = new CharacterStats(5, 2, 10, 0, 0, 0);
		currentHealth = maxHealth;
		controller = GetComponent<EnemyController2D>();
		animator = GetComponent<Animator>();
	}

	void Update()
	{
		TryAttack();
	}


	public void TryAttack()
	{
		
		if (controller.targetPlayer != null)
		{
			if (Vector3.Distance(controller.targetPlayer.position, transform.position) <= attackDistance)
			{
				animator.SetBool("isAttacking", true);
			}
			else
			{
				animator.SetBool("isAttacking", false);
			}
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			collision.GetComponent<Player>().TakeDamage(characterStats.GetStat(BaseStat.BaseStatType.Damage).GetCalculatedStatValue());
			Debug.Log($"DmgDealt = {characterStats.GetStat(BaseStat.BaseStatType.Damage).GetCalculatedStatValue()}");
		}
		Debug.Log($"Collision entered: {collision.name}");
	}

	public void PerformAttack()
	{

	}

	[Server]
	public void CmdTakeDamage(int amount)
	{
		currentHealth -= amount;
		animator.SetTrigger("Hit");
		if (currentHealth <= 0)
		{
			currentHealth = 0;
			animator.SetBool("isDead", true);
		}
	}

	public void OnChangedHealth(int oldHealth, int newHealth)
	{
		healthBar.value = newHealth;
	}

	[Server]
	public void CmdDie()
	{
		NetworkServer.Destroy(gameObject);
	}
}
