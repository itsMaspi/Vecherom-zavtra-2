using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Pistol : MonoBehaviour, IWeapon, IProjectileWeapon
{
	private Animator animator;
	public List<BaseStat> Stats { get; set; }
	public CharacterStats CharacterStats { get; set; }
	public Transform ProjectileSpawn { get; set; }
	LaserBullet laserBullet;

	void Start()
	{
		animator = GetComponent<Animator>();
		CharacterStats = transform.parent.parent.GetComponent<Player>().characterStats;
		ProjectileSpawn = transform.GetChild(0);
		laserBullet = Resources.Load<LaserBullet>("Weapons/Projectiles/pistol_j_bullet");
	}

	public void PerformAttack()
	{
		//Debug.Log("Attack performed");

		animator.SetTrigger("Shoot");

		//Debug.Log("Attack performed 2");
		//animator.SetBool("isShooting", isShooting);
	}

	public GameObject CastProjectile()
	{
		LaserBullet bulletInstance = Instantiate(laserBullet, transform.GetChild(0).position, Quaternion.identity);
		bulletInstance.Force = transform.parent.parent.lossyScale.normalized;
		bulletInstance.Speed = 300f;
		//bulletInstance.Damage = transform.GetComponentInParent<Player>().characterStats.GetStat(BaseStat.BaseStatType.Damage).GetCalculatedStatValue();
		bulletInstance.Range = 20f;
		return bulletInstance.gameObject;
		//NetworkServer.Spawn(bulletInstance.gameObject);
	}

	public void Shoot()
    {
		transform.GetComponentInParent<PlayerWeaponController>().CmdShoot(transform.GetComponentInParent<Player>().characterStats.GetStat(BaseStat.BaseStatType.Damage).GetCalculatedStatValue());
	}

}
