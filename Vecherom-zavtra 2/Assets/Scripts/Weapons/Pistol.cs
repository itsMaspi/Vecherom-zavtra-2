using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

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
		Debug.Log("Attack performed");

		animator.SetTrigger("Shoot");

		Debug.Log("Attack performed 2");
		//animator.SetBool("isShooting", isShooting);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Enemy")
		{
			Debug.Log($"Hit: {collision.name}");
			collision.GetComponent<IEnemy>().TakeDamage(Stats[0].GetCalculatedStatValue());
		}
	}

	public GameObject CastProjectile()
	{
		LaserBullet bulletInstance = Instantiate(laserBullet, transform.GetChild(0).position, transform.GetChild(0).rotation);
		bulletInstance.Force = transform.parent.parent.lossyScale.normalized;
		bulletInstance.Speed = 300f;
		bulletInstance.Damage = 5;
		bulletInstance.Range = 2f;
		return bulletInstance.gameObject;
		//NetworkServer.Spawn(bulletInstance.gameObject);
	}
}
