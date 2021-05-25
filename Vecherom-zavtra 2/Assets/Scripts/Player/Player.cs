using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    public CharacterStats characterStats;
    public int maxHealth = 100;
    [SyncVar(hook = nameof(OnChangedHealth))]
    public int currentHealth;

    public Slider healthSlider;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public override void OnStartLocalPlayer()
	{
		characterStats = new CharacterStats(10, 10, 10);
        healthSlider.maxValue = maxHealth;
	}

    [Server]
    public void TakeDamage(int ammount)
	{
        currentHealth -= ammount;
        if (currentHealth <= 0)
		{
            currentHealth = 0;
            GetComponent<Animator>().SetTrigger("Death");
		}
	}

    public void OnChangedHealth(int oldHealth, int newHealth)
	{
        healthSlider.value = newHealth;
	}

    [Command]
    public void CmdDestroySelf()
	{
        Destroy(gameObject);
	}
}
