using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LaserBullet : NetworkBehaviour
{
	public float Range { get; set; }
	public int Damage { get; set; }
	public float Speed { get; set; }
	public Vector3 Force { get; set; }

	private Vector3 spawnPosition;

    public override void OnStartServer()
    {
		Invoke(nameof(DestroySelf), Range);
    }

    // Start is called before the first frame update
    void Start()
	{

		GetComponent<Rigidbody2D>().AddForce(Force * Speed); // el fill no es mou, transform.right sempre es positiu
		//spawnPosition = transform.position;
		//Debug.Log(Force * Speed);
    }

	//void Update()
	//{
	//	if (Vector3.Distance(spawnPosition, transform.position) >= Range)
	//	{
	//		Remove();
	//	}
	//}

	[Server]
	void DestroySelf()
    {
		NetworkServer.Destroy(gameObject);
    }

	[ServerCallback]
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Enemy")
		{
			Debug.Log($"Hit: {collision.name}");
			collision.GetComponent<IEnemy>().TakeDamage(Damage);
		}
		AudioManager.instance.Play("bulletHit");
		NetworkServer.Destroy(gameObject);
		//Remove();
	}

	/*
	public void Remove()
	{
		Destroy(gameObject);
		NetworkServer.UnSpawn(gameObject); // command?
	}
	*/
}
