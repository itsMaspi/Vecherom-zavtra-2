using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemySpawnerController : NetworkBehaviour
{
	public override void OnStartServer()
	{
		base.OnStartServer();

		Invoke(nameof(SpawnEnemy), 10f);
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	void SpawnEnemy()
	{
		GameObject enemy = Instantiate(Resources.Load<GameObject>($"Enemies/WildBeast"), transform.position, transform.rotation);
		NetworkServer.Spawn(enemy);
	}
}
