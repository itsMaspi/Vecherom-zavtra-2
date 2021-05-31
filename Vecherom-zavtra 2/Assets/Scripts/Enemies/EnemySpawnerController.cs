using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemySpawnerController : NetworkBehaviour
{
	public Transform[] SpawnPositions;

	public GameObject[] enemyPrefabs;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
			CmdSpawnEnemy();
        }
    }

	[Command(requiresAuthority = false)]
	void CmdSpawnEnemy()
	{
		Transform spawnPos = transform;
		if (SpawnPositions.Length > 0)
		{
			spawnPos = SpawnPositions[Random.Range(0, SpawnPositions.Length)];
		}
		foreach (GameObject prefab in enemyPrefabs)
		{
			GameObject enemy = Instantiate(prefab, spawnPos.position, spawnPos.rotation);
			NetworkServer.Spawn(enemy);
			spawnPos.position = new Vector3(spawnPos.position.x + 5f, spawnPos.position.y, spawnPos.position.z);
		}
		NetworkServer.Destroy(gameObject);
	}


}
