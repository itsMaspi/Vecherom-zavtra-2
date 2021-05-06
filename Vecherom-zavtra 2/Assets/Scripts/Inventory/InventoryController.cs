using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class InventoryController : NetworkBehaviour
{
	[HideInInspector]public PlayerWeaponController playerWeaponController;
	public ConsumableController consumableController;
	public Item pistol;
	public Item PotionLog;

	public override void OnStartLocalPlayer()
	{
		playerWeaponController = GetComponent<PlayerWeaponController>();
		List<BaseStat> pistolStats = new List<BaseStat>();
		pistolStats.Add(new BaseStat(6, "Attack", "The attack power"));
		pistol = new Item(pistolStats, "pistol");

		PotionLog = new Item(new List<BaseStat>(), "potion_log", "Drink this to log something cool!", "Drink", "Log Potion", false);
	}

	public void OnEquipWeapon(InputValue value)
	{
		if (!isLocalPlayer) return;
		if (value.isPressed)
		{
			//playerWeaponController.EquipWeapon(pistol);
			consumableController.ConsumeItem(PotionLog);
		}
	}
}
