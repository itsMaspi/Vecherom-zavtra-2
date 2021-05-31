using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using kcp2k;

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
		characterStats = new CharacterStats(10, 10, 10, 0, 0, 0);
        healthSlider.maxValue = maxHealth;
	}

	public override void OnStartServer()
	{
        currentHealth = maxHealth;
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

    [Command]
    public void CmdHeal(int ammount)
    {
        currentHealth += ammount;
        if (currentHealth >= 100)
        {
            currentHealth = 100;
        }
    }

    public void ResetServer()
	{
        NetworkServer.DisconnectAll();
	}

    public void OnChangedHealth(int oldHealth, int newHealth)
	{
        healthSlider.value = newHealth;
	}

    [Command]
    public void CmdDestroySelf()
    {
        RpcDie();
        //NetworkServer.Destroy(gameObject);
	}

    [ClientRpc]
    public void RpcDie()
	{
        var nm = FindObjectOfType<NetworkManager>();
        nm.StopClient();
        nm.StopHost();
        nm.StopServer();
        FindObjectOfType<SceneLoader>().LoadLevel(SceneManager.GetActiveScene().name);
    }
}
