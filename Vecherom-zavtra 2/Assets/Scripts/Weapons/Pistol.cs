using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour, IWeapon, IProjectileWeapon
{
	private Animator animator;
	public List<BaseStat> Stats { get; set; }
	public Transform ProjectileSpawn { get; set; }
	LaserBullet laserBullet;

	void Start()
	{
		animator = GetComponent<Animator>();
		ProjectileSpawn = transform.GetChild(0);
		laserBullet = Resources.Load<LaserBullet>("Weapons/Projectiles/laser_bullet");
	}

	public void PerformAttack()
	{
		animator.SetTrigger("Attack");
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
		//laserBullet.Speed = 50f;
		LaserBullet bulletInstance = Instantiate(laserBullet, ProjectileSpawn.position, ProjectileSpawn.rotation);
		bulletInstance.Speed = 50f;
	}
}
