using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
	public string[] dialogue;
	public string npcName;

	public PlayerWeaponController playerWeaponController;
	public Item sword;

	public override void Interact()
	{
		DialogueManager.Instance.AddNewDialogue(dialogue, npcName);
		List<BaseStat> swordStats = new List<BaseStat>();
		swordStats.Add(new BaseStat(6, "Attack", "The attack power"));
		sword = new Item(swordStats, "sword");
		playerWeaponController.EquipWeapon(sword);
	}
}
