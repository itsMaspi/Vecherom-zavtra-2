using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class InventoryController : NetworkBehaviour
{
	public static InventoryController Instance { get; set; }

	[HideInInspector] public PlayerWeaponController playerWeaponController;
	[HideInInspector] public ConsumableController consumableController;
	public Item pistol;
	public Item PotionLog;

	public override void OnStartLocalPlayer()
	{
		playerWeaponController = GetComponent<PlayerWeaponController>();
		consumableController = GetComponent<ConsumableController>();
		List<BaseStat> pistolStats = new List<BaseStat>();
		pistolStats.Add(new BaseStat(6, "Attack", "The attack power"));
		pistol = new Item(pistolStats, "pistol");

		PotionLog = new Item(new List<BaseStat>(), "potion_log", "Drink this to log something cool!", "Drink", "Log Potion", false);
	}

	void Start()
	{
		if (Instance != null && Instance != this)
			Destroy(gameObject);
		else
			Instance = this;

	}
}
