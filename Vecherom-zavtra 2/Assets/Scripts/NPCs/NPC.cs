using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
	public string[] dialogue;
	public string npcName;

	public override void Interact()
	{
		DialogueManager.Instance.AddNewDialogue(dialogue, npcName);
	}
}
