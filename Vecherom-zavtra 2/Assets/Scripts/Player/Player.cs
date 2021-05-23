using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    public CharacterStats characterStats;
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public override void OnStartLocalPlayer()
	{
		characterStats = new CharacterStats(10, 10, 10);
	}

    public void TakeDamage(int ammount)
	{
        currentHealth -= ammount;
	}
}
