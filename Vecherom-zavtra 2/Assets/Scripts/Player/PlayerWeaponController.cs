using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerWeaponController : NetworkBehaviour
{
	public GameObject weaponPoint;
	
	public GameObject EquippedWeapon;

	public Animator pistolAnimator;

	public Animator playerAnimator;



	IWeapon equippedWeapon;
	CharacterStats characterStats;

	public override void OnStartLocalPlayer()
	{
		characterStats = GetComponent<CharacterStats>();
		
		weaponPoint = transform.Find("WeaponPoint").gameObject;

		//playerAnimator = GetComponent<Animator>();
		/*Debug.Log(weaponPoint.transform.GetChild(0));
		Debug.Log(weaponPoint.transform.GetChild(0).gameObject.name);
		EquippedWeapon = weaponPoint.transform.GetChild(0).gameObject;
		Debug.Log(EquippedWeapon.GetComponent<Animator>());
		animator = EquippedWeapon.GetComponent<Animator>();

		equippedWeapon = EquippedWeapon.GetComponent<IWeapon>();
		equippedWeapon.Stats = new List<BaseStat>();
		//EquippedWeapon.transform.SetParent(weaponPoint.transform); necessari ?????
		characterStats.AddStatBonus(new List<BaseStat>());*/
	}

	/*void Start()
	{
		characterStats = GetComponent<CharacterStats>();

		weaponPoint = transform.Find("WeaponPoint").gameObject;
		EquippedWeapon = weaponPoint.transform.GetChild(0).gameObject;
		animator = EquippedWeapon.GetComponent<Animator>();

		equippedWeapon = EquippedWeapon.GetComponent<IWeapon>();
		equippedWeapon.Stats = new List<BaseStat>();
		//EquippedWeapon.transform.SetParent(weaponPoint.transform); necessari ?????
		characterStats.AddStatBonus(new List<BaseStat>());
	}*/

	public void EquipWeapon(Item itemToEquip) // REFACTOR: EquipItem()
	{
		if (!isLocalPlayer) return;
		if (EquippedWeapon != null)
		{
			characterStats.RemoveStatBonus(EquippedWeapon.GetComponent<IWeapon>().Stats);
			Destroy(weaponPoint.transform.GetChild(0).gameObject);
		}
		EquippedWeapon = Instantiate(Resources.Load<GameObject>($"Weapons/{itemToEquip.ObjectSlug}"), weaponPoint.transform);
		equippedWeapon = EquippedWeapon.GetComponent<IWeapon>();
		equippedWeapon.Stats = itemToEquip.Stats;
		//EquippedWeapon.transform.SetParent(weaponPoint.transform); necessari ?????
		characterStats.AddStatBonus(itemToEquip.Stats);
		//NetworkServer.Spawn(EquippedWeapon);
		//SpawnWeapon(EquippedWeapon);
		//Debug.Log(characterStats.stats[1].GetCalculatedStatValue());
	}

	//[ClientRpc]
	public void RpcEquipWeapon(Item itemToEquip)
	{
		if (!isLocalPlayer) return;
		if (EquippedWeapon != null)
		{
			characterStats.RemoveStatBonus(EquippedWeapon.GetComponent<IWeapon>().Stats);
			Destroy(weaponPoint.transform.GetChild(0).gameObject);
		}
		EquippedWeapon = Instantiate(Resources.Load<GameObject>($"Weapons/{itemToEquip.ObjectSlug}"), weaponPoint.transform);
		equippedWeapon = EquippedWeapon.GetComponent<IWeapon>();
		equippedWeapon.Stats = itemToEquip.Stats;
		characterStats.AddStatBonus(itemToEquip.Stats);
	}

	public void OnAttack(InputValue value)
	{
		if (!isLocalPlayer) return;
		/*if (value.isPressed)
		{
			equippedWeapon.PerformAttack();
		}*/
		//equippedWeapon.PerformAttack(value.isPressed);
		if (PauseManager.pauseState == PauseState.Paused) return;

		
		playerAnimator.SetBool("isShooting", value.isPressed);

	}

	[Command]
	public void CmdShoot()
	{
		//equippedWeapon.PerformAttack();
		/*LaserBullet bulletInstance = Instantiate(Resources.Load<LaserBullet>("Weapons/Projectiles/laser_bullet"), EquippedWeapon.transform.GetChild(0).position, EquippedWeapon.transform.GetChild(0).rotation);
		bulletInstance.Force = transform.lossyScale.normalized;
		bulletInstance.Speed = 300f;
		bulletInstance.Damage = 5;
		bulletInstance.Range = 20f;*/



		GameObject bulletInstance = EquippedWeapon.GetComponent<IProjectileWeapon>().CastProjectile();
		NetworkServer.Spawn(bulletInstance);
		RpcShoot();
	}

	/* FER QUE EL PLAYER TINGUI UN CLIP DE DISPARAR AMB UN TRIGGER QUE CRIDI LA FUNCIO ShootAnim */

	[ClientRpc]
	public void RpcShoot()
	{
		playerAnimator.speed = 0.5f;
		pistolAnimator.speed = 0.5f;
		pistolAnimator.SetTrigger("Shoot"); //EquippedWeapon.GetComponent<Animator>()
	}

	/*[Command]
	public void ShootAnim()
	{
		GameObject bulletInstance = EquippedWeapon.GetComponent<IProjectileWeapon>().CastProjectile();
		NetworkServer.Spawn(bulletInstance);
	}*/
}
