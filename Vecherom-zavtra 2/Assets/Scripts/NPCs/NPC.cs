using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
	public int npcId;
	public int dialogueId;

	public override void Interact()
	{
		DialogueManager.Instance.AddNewDialogue(npcId, dialogueId);
	}
}
