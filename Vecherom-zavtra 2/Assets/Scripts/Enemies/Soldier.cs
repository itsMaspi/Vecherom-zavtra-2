using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : NetworkBehaviour, IEnemy
{
	[SyncVar(hook = nameof(OnChangedHealth))]
	public int currentHealth;
	public int maxHealth;
	[SerializeField] float attackDistance = 5f;
	[SerializeField] private LayerMask whatCanDamage;
	[SerializeField] float attackCooldown = 1f;
	float attackTime = 0f;

	public Animator animator;
	public TMPro.TextMeshProUGUI healthBar;

	private CharacterStats characterStats;
	public EnemyController2D controller;

	public void Awake()
	{
		characterStats = new CharacterStats(6, 2, 10);
		currentHealth = maxHealth;
		controller = GetComponent<EnemyController2D>();
		animator = GetComponent<Animator>();
	}

	void Update()
	{
		if (!isServer) return;
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
			animator.SetTrigger("Death");
		}
	}

	public void OnChangedHealth(int oldHealth, int newHealth)
	{
		healthBar.text = newHealth.ToString();
	}

	[Server]
	public void CmdDie()
	{
		NetworkServer.Destroy(gameObject);
	}
}
