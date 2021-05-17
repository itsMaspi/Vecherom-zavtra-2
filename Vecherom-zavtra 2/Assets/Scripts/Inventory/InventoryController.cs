using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Mirror;

public class InventoryController : NetworkBehaviour
{
	public static InventoryController Instance { get; set; }

	[HideInInspector] public PlayerWeaponController playerWeaponController;
	[HideInInspector] public ConsumableController consumableController;
	public InventoryUIDetails inventoryDetailsPanel;

	public List<Item> playerItems = new List<Item>();

	public Item pistol;
	public Item PotionLog;


	public override void OnStartLocalPlayer()
	{
		/*playerWeaponController = GetComponent<PlayerWeaponController>();
		consumableController = GetComponent<ConsumableController>();

		GiveItem("j_pistol");*/
		//List<BaseStat> pistolStats = new List<BaseStat>();
		//pistolStats.Add(new BaseStat(6, "Attack", "The attack power"));
		//pistol = new Item(pistolStats, "pistol");

		//PotionLog = new Item(new List<BaseStat>(), "potion_log", "Drink this to log something cool!",Item.ItemTypes.Consumable, "Drink", "Log Potion", false);
	}

	void Start()
	{
		if (Instance != null && Instance != this)
			Destroy(gameObject);
		else
			Instance = this;

		playerWeaponController = GetComponent<PlayerWeaponController>();
		consumableController = GetComponent<ConsumableController>();

		GiveItem("pistol_j");
		GiveItem("pistol_b");
		GiveItem("pistol_m");
	}

	public void GiveItem(string itemSlug)
	{
		Item item = ItemDatabase.Instance.GetItem(itemSlug);
		playerItems.Add(item);
		Debug.Log($"{playerItems.Count} items in inventory. Added: {itemSlug}");
		UIEventHandler.ItemAddedToInventory(item);
	}

	public void SetItemDetails(Item item, Button selectedButton)
	{
		inventoryDetailsPanel.SetItem(item, selectedButton);
	}

	public void EquipItem(Item itemToEquip)
	{
		playerWeaponController.EquipWeapon(itemToEquip);
	}

	public void ConsumeItem(Item itemToConsume)
	{
		consumableController.ConsumeItem(itemToConsume);
	}
}
