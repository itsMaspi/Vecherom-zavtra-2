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

	private CharacterStats characterStats;
	public EnemyController2D controller;

	public override void OnStartServer()
	{
		base.OnStartServer();
		characterStats = new CharacterStats(6, 2, 10);
		currentHealth = maxHealth;
		//controller = GetComponent<EnemyController2D>();
		//animator = GetComponent<Animator>();
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
				//PerformAttack();
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



		/*
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackDistance, whatCanDamage);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].tag == "Player")
			{
				colliders[i].GetComponent<Player>().TakeDamage(characterStats.GetStat(BaseStat.BaseStatType.Damage).GetCalculatedStatValue());
				Debug.Log($"DmgDealt = {characterStats.GetStat(BaseStat.BaseStatType.Damage).GetCalculatedStatValue()}");
			}
		}
		attackTime = 0;
		*/
	}

	public void CmdTakeDamage(int amount)
	{
		currentHealth -= amount;
		Debug.Log($"CmdTakeDamage: Ouch! -{amount}hp");
		if (currentHealth <= 0)
		{
			Debug.Log($"CmdTakeDamage: Dead!");
			NetworkServer.UnSpawn(gameObject);
		}
	}

	public void OnChangedHealth(int oldHealth, int newHealth)
	{
		transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = newHealth.ToString();
		if (newHealth <= 0)
			Destroy(gameObject);
	}
}
