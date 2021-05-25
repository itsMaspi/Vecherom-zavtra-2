using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemySpawnerController : NetworkBehaviour
{
	public Transform[] SpawnPositions;
	public override void OnStartServer()
	{
		base.OnStartServer();

		//InvokeRepeating(nameof(SpawnEnemy), 3f, 5f);
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	void SpawnEnemy()
	{
		Transform spawnPos = transform;
		if (SpawnPositions.Length > 0)
		{
			spawnPos = SpawnPositions[Random.Range(0, SpawnPositions.Length)];
		}
		GameObject enemy = Instantiate(Resources.Load<GameObject>($"Enemies/Maliwan/Soldier"), spawnPos.position, spawnPos.rotation);
		NetworkServer.Spawn(enemy);
	}
}
