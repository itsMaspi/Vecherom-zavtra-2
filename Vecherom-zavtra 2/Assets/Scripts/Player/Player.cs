using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    public CharacterStats characterStats;

    void Start()
    {
        
    }
    public override void OnStartLocalPlayer()
	{
		characterStats = new CharacterStats(10, 10, 10);
	}
}
