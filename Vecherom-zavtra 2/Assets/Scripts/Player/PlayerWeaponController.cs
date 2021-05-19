using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerWeaponController : NetworkBehaviour
{
	public GameObject weaponPoint;

	[SyncVar(hook = nameof(OnChangeEquippedWeapon))]
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
		Debug.Log("Method EquipWeapon called :)");

		CmdEquipWeapon(itemToEquip.ObjectSlug);

		Debug.Log("Command CmdEquipWeapon finished.");


		//NetworkServer.Spawn(EquippedWeapon);
		//SpawnWeapon(EquippedWeapon);
		//Debug.Log(characterStats.stats[1].GetCalculatedStatValue());
	}

	public void OnChangeEquippedWeapon(string oldSlug, string newSlug)
	{
		StartCoroutine(ChangeEquippedWeapon(newSlug));
	}

	IEnumerator ChangeEquippedWeapon(string itemToEquip)
    {
		Debug.Log("Syncvar Hook called.");

		if (transform.Find("WeaponPoint").childCount > 0)
		{
			Debug.Log("Player already has a weapon equipped!");
			GetComponent<InventoryController>().GiveItem(itemToEquip);
			GetComponent<Player>().characterStats.RemoveStatBonus(GetComponent<ItemDatabase>().GetItem(itemToEquip).Stats);
			Destroy(transform.Find("WeaponPoint").GetChild(0).gameObject);
			//CmdServerUnpawnWeapon(weaponPoint.transform.GetChild(0).gameObject);
			Debug.Log("Player weapon destroyed.");
			yield return null;
		}

		EquippedWeapon = Instantiate(Resources.Load<GameObject>($"Weapons/{itemToEquip}"), transform.Find("WeaponPoint"));
		//CmdServerSpawnWeapon(EquippedWeapon);
		Debug.Log("Equipped weapon instanciated.");

		//equippedWeapon = EquippedWeapon.GetComponent<IWeapon>();
		EquippedWeapon.GetComponent<IWeapon>().Stats = GetComponent<ItemDatabase>().GetItem(itemToEquip).Stats;
		GetComponent<Player>().characterStats.AddStatBonus(EquippedWeapon.GetComponent<IWeapon>().Stats);
		Debug.Log("Syncvar Hook ended.");
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
