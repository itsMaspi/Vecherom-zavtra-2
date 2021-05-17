using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBeast : MonoBehaviour, IEnemy
{
	public float currentHealth, power, toughness;
	public float maxHealth;

	private CharacterStats characterStats;

	void Start()
	{
		characterStats = new CharacterStats(6, 2, 10);
		currentHealth = maxHealth;
	}

	public void PerformAttack()
	{
		throw new System.NotImplementedException();
	}

	public void TakeDamage(int amount)
	{
		Debug.Log($"Ouch! -{amount}hp");
		currentHealth -= amount;
		if (currentHealth <= 0) 
		{
			Die();
		}
	}

	void Die()
	{
		Destroy(gameObject);
	}
}
