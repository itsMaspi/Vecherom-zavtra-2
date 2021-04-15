using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
	[HideInInspector]public PlayerWeaponController playerWeaponController;
	public Item pistol;

	void Start()
	{
		playerWeaponController = GetComponent<PlayerWeaponController>();
		List<BaseStat> pistolStats = new List<BaseStat>();
		pistolStats.Add(new BaseStat(6, "Attack", "The attack power"));
		pistol = new Item(pistolStats, "pistol");
	}

	public void EquipWeapon(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			playerWeaponController.EquipWeapon(pistol);
		}
	}
}
