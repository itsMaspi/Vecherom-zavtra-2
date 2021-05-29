using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Pistol : NetworkBehaviour, IWeapon, IProjectileWeapon
{
	private Animator animator;
	public List<BaseStat> Stats { get; set; }
	public CharacterStats CharacterStats { get; set; }
	public Transform ProjectileSpawn { get; set; }

	public string bulletName;

	LaserBullet laserBullet;

	void Start()
	{
		animator = GetComponent<Animator>();
		CharacterStats = transform.parent.parent.GetComponent<Player>().characterStats;
		ProjectileSpawn = transform.GetChild(0);
		laserBullet = Resources.Load<LaserBullet>($"Weapons/Projectiles/{bulletName}");
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
		Debug.Log($"5 {laserBullet}");

		LaserBullet bulletInstance = Instantiate(laserBullet, transform.GetChild(0).position, Quaternion.identity);
		Debug.Log("8");
		//Spread
		bulletInstance.Force = transform.parent.parent.lossyScale.normalized;
		Debug.Log("10");
		//bulletInstance.Speed = Stats.Find(x => x.StatType == BaseStat.BaseStatType.Speed).BaseValue;
		Debug.Log("20");

		//bulletInstance.Damage = transform.GetComponentInParent<Player>().characterStats.GetStat(BaseStat.BaseStatType.Damage).GetCalculatedStatValue();
		//bulletInstance.Range = Stats.Find(x => x.StatType == BaseStat.BaseStatType.Range).BaseValue;
		return bulletInstance.gameObject;
		//NetworkServer.Spawn(bulletInstance.gameObject);
	}

	public void Shoot()
	{
		var dmg = Stats.Find(x => x.StatType == BaseStat.BaseStatType.Damage).BaseValue;
		var speed = Stats.Find(x => x.StatType == BaseStat.BaseStatType.Speed).BaseValue;
		var range = Stats.Find(x => x.StatType == BaseStat.BaseStatType.Range).BaseValue;
		transform.GetComponentInParent<PlayerWeaponController>().CmdShoot(dmg, speed, range);
	}

	public void StopShoot()
	{
		transform.parent.parent.GetComponent<Animator>().SetBool("isShooting", false);
	}

}
