using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
	[HideInInspector]public PlayerWeaponController playerWeaponController;
    public Item sword;

	void Start()
	{
		playerWeaponController = GetComponent<PlayerWeaponController>();
		List<BaseStat> swordStats = new List<BaseStat>();
		swordStats.Add(new BaseStat(6, "Attack", "The attack power"));
		sword = new Item(swordStats, "sword");
	}

	public void EquipWeapon(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			playerWeaponController.EquipWeapon(sword);
		}
	}
}
