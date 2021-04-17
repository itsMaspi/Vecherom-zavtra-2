using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class InventoryController : NetworkBehaviour
{
	[HideInInspector]public PlayerWeaponController playerWeaponController;
	public Item pistol;

	public override void OnStartLocalPlayer()
	{
		playerWeaponController = GetComponent<PlayerWeaponController>();
		List<BaseStat> pistolStats = new List<BaseStat>();
		pistolStats.Add(new BaseStat(6, "Attack", "The attack power"));
		pistol = new Item(pistolStats, "pistol");
	}

	public void OnEquipWeapon(InputValue value)
	{
		if (!isLocalPlayer) return;
		if (value.isPressed)
		{
			playerWeaponController.EquipWeapon(pistol);
		}
	}
}
