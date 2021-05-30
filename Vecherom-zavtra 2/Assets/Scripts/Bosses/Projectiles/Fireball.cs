using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Fireball : NetworkBehaviour
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

	void Start()
	{
		GetComponent<Rigidbody2D>().AddForce(Force * Speed); // el fill no es mou, transform.right sempre es positiu


	}

    void Update()
    {
		float angle = Mathf.Atan2(GetComponent<Rigidbody2D>().velocity.y, GetComponent<Rigidbody2D>().velocity.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		var angles = transform.eulerAngles;
		if (GetComponent<Rigidbody2D>().velocity.x < 0)
        {
			angles.z += 180;
		}
		transform.eulerAngles = angles;
	}


    [Server]
	void DestroySelf()
	{
		NetworkServer.Destroy(gameObject);
	}

	[ServerCallback]
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "EnemyProjectile") return;
		if (collision.tag == "Player")
		{
			Debug.Log($"Hit: {collision.name}");
			collision.GetComponent<Player>().TakeDamage(Damage);
		}
		CmdFireballHit();
		NetworkServer.Destroy(gameObject);
		//Remove();
	}

	[Command(requiresAuthority = false)]
	public void CmdFireballHit()
	{
		GameObject hit = (GameObject)Instantiate(Resources.Load("Bosses/Projectiles/Fireball_hit"), transform.position, Quaternion.identity);


		Vector3 theScale = hit.transform.localScale;
		theScale.x *= Force.x;
		hit.transform.localScale = theScale;

		NetworkServer.Spawn(hit);
		//RPC Client animation
	}


}
