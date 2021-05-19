using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerWeaponController : NetworkBehaviour
{
	public GameObject weaponPoint;

	[SyncVar(hook = nameof(ChangeEquippedWeapon))]
	public string EquippedWeaponSlug;
	public GameObject EquippedWeapon;

	public Animator pistolAnimator;

	public Animator playerAnimator;

	IWeapon equippedWeapon;
	CharacterStats characterStats;

	public override void OnStartLocalPlayer()
	{
		characterStats = GetComponent<Player>().characterStats;
		
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

		CmdEquipWeapon(itemToEquip.ObjectSlug);


		
		//NetworkServer.Spawn(EquippedWeapon);
		//SpawnWeapon(EquippedWeapon);
		//Debug.Log(characterStats.stats[1].GetCalculatedStatValue());
	}

	public void ChangeEquippedWeapon(string oldSlug, string newSlug)
	{
		if (EquippedWeapon != null)
		{
			GetComponent<InventoryController>().GiveItem(EquippedWeapon.name.Replace("(Clone)", ""));
			GetComponent<Player>().characterStats.RemoveStatBonus(EquippedWeapon.GetComponent<IWeapon>().Stats);
			//Destroy(weaponPoint.transform.GetChild(0).gameObject);
			CmdServerUnpawnWeapon(weaponPoint.transform.GetChild(0).gameObject);
		}
		EquippedWeapon = Instantiate(Resources.Load<GameObject>($"Weapons/{newSlug}"), weaponPoint.transform);
		CmdServerSpawnWeapon(EquippedWeapon);
		Debug.Log("Syncvar Hook called.");
		equippedWeapon = EquippedWeapon.GetComponent<IWeapon>();


		equippedWeapon.Stats = GetComponent<ItemDatabase>().GetItem(newSlug).Stats;
		GetComponent<Player>().characterStats.AddStatBonus(GetComponent<ItemDatabase>().GetItem(newSlug).Stats);
	}

	[Command]
	public void CmdEquipWeapon(string slug)
	{
		EquippedWeaponSlug = slug;
	}

	[Command]
	public void CmdServerSpawnWeapon(GameObject Weapon)
	{
		NetworkServer.Spawn(Weapon);

	}

	[Command]
	public void CmdServerUnpawnWeapon(GameObject Weapon)
	{
		NetworkServer.UnSpawn(Weapon);

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
		if (EquippedWeapon == null) return; 

		
		GetComponent<Animator>().SetBool("isShooting", value.isPressed);

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
		weaponPoint.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Shoot"); //EquippedWeapon.GetComponent<Animator>()
		//animator.SetTrigger("Shoot");
	}

	/*[Command]
	public void ShootAnim()
	{
		GameObject bulletInstance = EquippedWeapon.GetComponent<IProjectileWeapon>().CastProjectile();
		NetworkServer.Spawn(bulletInstance);
	}*/
}
