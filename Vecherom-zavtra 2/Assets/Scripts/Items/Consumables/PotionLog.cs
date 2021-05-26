using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionLog : MonoBehaviour, IConsumable
{
	public void Consume()
	{
		Debug.Log("Potion logged");
		Destroy(gameObject);
	}

	public void Consume(CharacterStats stats)
	{
		Debug.Log("Potion logged (stats)");
	}

    public void Consume(GameObject gameObject)
    {
		gameObject.GetComponent<Player>().CmdHeal(50);
    }
}
