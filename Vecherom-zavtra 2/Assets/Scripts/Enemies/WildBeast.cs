using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WildBeast : NetworkBehaviour, IEnemy
{
	[SyncVar(hook = nameof(OnChangedHealth))]
	public int currentHealth;
	public int maxHealth;
	[SerializeField] float attackDistance = 5f;

	private CharacterStats characterStats;
	private EnemyController2D controller;

	public override void OnStartServer()
	{
		base.OnStartServer();
		characterStats = new CharacterStats(6, 2, 10);
		currentHealth = maxHealth;
		controller = GetComponent<EnemyController2D>();
	}

	void Update()
	{
		if (!isServer) return;
		if (controller.targetPlayer != null && Vector3.Distance(controller.targetPlayer.position, transform.position) <= attackDistance)
		{
			PerformAttack();
		}
	}

	public void PerformAttack()
	{
		throw new System.NotImplementedException();
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
