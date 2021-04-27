using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : Interactable
{
	public string SceneName;

	public override void Interact()
	{
		SceneManager.LoadScene(SceneName);
	}
}
