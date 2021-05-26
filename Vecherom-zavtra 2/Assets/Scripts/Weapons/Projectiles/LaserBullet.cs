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
		Vector3 f = new Vector3(Force.x, 0f, Force.z);
		GetComponent<Rigidbody2D>().AddForce(f * Speed); // el fill no es mou, transform.right sempre es positiu
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
			collision.GetComponent<IEnemy>().CmdTakeDamage(Damage);
			CmdLaserHit();
			NetworkServer.Destroy(gameObject);
		}
		//Remove();
	}

	[Command(requiresAuthority = false)]
	public void CmdLaserHit()
    {
		GameObject hit = (GameObject)Instantiate(Resources.Load("Weapons/Projectiles/proj1_hit"), transform.position, transform.rotation);


		Vector3 theScale = hit.transform.localScale;
		theScale.x *= Force.x;
		hit.transform.localScale = theScale;

		NetworkServer.Spawn(hit);
		//RPC Client animation
	}

	

	/*
	public void Remove()
	{
		Destroy(gameObject);
		NetworkServer.UnSpawn(gameObject); // command?
	}
	*/
}
