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

    public override void OnStartServer()
    {
		Invoke(nameof(DestroySelf), Range);
    }

	public LaserBullet(float Range, int Damage, float Speed, Vector3 Force)
	{

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
			collision.GetComponent<IEnemy>().CmdTakeDamage(Damage);
		}
		CmdLaserHit();
		NetworkServer.Destroy(gameObject);
		//Remove();
	}

	//[Command(requiresAuthority = false)]
	public void CmdLaserHit()
    {
		GameObject hit = (GameObject)Instantiate(Resources.Load("Weapons/Projectiles/pistol_j_hit"), transform.position, transform.rotation);


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

public static class CustomReadWriteFunctions
{
	public static void WriteLaserBullet(this NetworkWriter writer, LaserBullet value)
	{
		writer.WriteSingle(value.Range);
		writer.WriteInt32(value.Damage);
		writer.WriteSingle(value.Speed);
		writer.WriteVector3(value.Force);
	}

	public static LaserBullet ReadLaserBullet(this NetworkReader reader)
	{
		return new LaserBullet(reader.ReadSingle(), reader.ReadInt32(), reader.ReadSingle(), reader.ReadVector3());
	}
}