using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Pistol : NetworkBehaviour, IWeapon, IProjectileWeapon
{
	private Animator animator;
	public List<BaseStat> Stats { get; set; }
	public Transform ProjectileSpawn { get; set; }
	LaserBullet laserBullet;

	/*public override void OnStartLocalPlayer()
	{
		animator = GetComponent<Animator>();
		ProjectileSpawn = transform.GetChild(0);
		laserBullet = Resources.Load<LaserBullet>("Weapons/Projectiles/laser_bullet");
	}*/

	void Start()
	{
		animator = GetComponent<Animator>();
		ProjectileSpawn = transform.GetChild(0);
		laserBullet = Resources.Load<LaserBullet>("Weapons/Projectiles/laser_bullet");
	}

	public void PerformAttack(bool isShooting)
	{
		//animator.SetTrigger("Shoot");
		animator.SetBool("isShooting", isShooting);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Enemy")
		{
			Debug.Log($"Hit: {collision.name}");
			collision.GetComponent<IEnemy>().TakeDamage(Stats[0].GetCalculatedStatValue());
		}
	}

	public void CastProjectile()
	{
		LaserBullet bulletInstance = Instantiate(laserBullet, ProjectileSpawn.position, ProjectileSpawn.rotation);
		bulletInstance.Force = transform.parent.parent.lossyScale.normalized;
		bulletInstance.Speed = 300f;
		bulletInstance.Damage = 5;
		bulletInstance.Range = 20f;
		//NetworkServer.Spawn(bulletInstance.gameObject);
		SpawnProjectile(bulletInstance.gameObject);
	}

	[Command]
	public void SpawnProjectile(GameObject projectile)
	{
		if (isServer) return;
		NetworkServer.Spawn(projectile, transform.parent.parent.gameObject);
	}
}
