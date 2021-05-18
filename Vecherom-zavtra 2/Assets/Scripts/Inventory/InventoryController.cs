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

	public List<Item> playerItems;

	public Item pistol;
	public Item PotionLog;


	public override void OnStartLocalPlayer()
	{
		playerItems = new List<Item>();
		GiveItem("pistol_j");
		GiveItem("pistol_b");
		GiveItem("pistol_m");
		GiveItem("potion_log");
	}

	void Start()
	{
		if (Instance != null && Instance != this)
			Destroy(gameObject);
		else
			Instance = this;
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
		GetComponent<PlayerWeaponController>().EquipWeapon(itemToEquip);
	}

	public void ConsumeItem(Item itemToConsume)
	{
		GetComponent<ConsumableController>().ConsumeItem(itemToConsume);
	}
}
