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

	}

	void Start()
	{

	}

	public void EquipWeapon(Item itemToEquip) // REFACTOR: EquipItem()
	{
		if (!isLocalPlayer) return;
		//Debug.Log("Method EquipWeapon called :)");

		CmdEquipWeapon(itemToEquip.ObjectSlug);

		//Debug.Log("Command CmdEquipWeapon finished.");

	}

	public void OnChangeEquippedWeapon(string oldSlug, string newSlug)
	{
		StartCoroutine(ChangeEquippedWeapon(oldSlug, newSlug));
	}

	public void DropItem()
	{
		var itemSlug = GetComponent<InventoryUI>().inventoryPanel.Find("Inventory_Details").GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text;
		CmdDropItem(itemSlug);
	}

	[Command]
	public void CmdDropItem(string itemSlug)
    {
		GameObject weapon = Instantiate<GameObject>(Resources.Load<GameObject>($"Drops/{itemSlug}_drop"), transform.position, transform.rotation);

		NetworkServer.Spawn(weapon);
    }

	IEnumerator ChangeEquippedWeapon(string oldItem ,string itemToEquip)
    {

		if (transform.Find("WeaponPoint").childCount > 0)
		{
			GetComponent<InventoryController>().GiveItem(oldItem);
			if (isLocalPlayer)
				GetComponent<Player>().characterStats.RemoveStatBonus(GetComponent<ItemDatabase>().GetItem(oldItem).Stats);
			Destroy(transform.Find("WeaponPoint").GetChild(0).gameObject);
			yield return null;
		}

		EquippedWeapon = Instantiate(Resources.Load<GameObject>($"Weapons/{itemToEquip}"), transform.Find("WeaponPoint"));

		if (isLocalPlayer)
		{
			EquippedWeapon.GetComponent<IWeapon>().Stats = GetComponent<ItemDatabase>().GetItem(itemToEquip).Stats;
			GetComponent<Player>().characterStats.AddStatBonus(EquippedWeapon.GetComponent<IWeapon>().Stats);
		}
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
		if (PauseManager.pauseState == PauseState.Paused) return;
		if (GetComponent<PlayerController>().isChatting) return;
		if (EquippedWeapon == null) return;

		
		GetComponent<Animator>().SetBool("isShooting", value.isPressed);

	}

	[Command]
	public void CmdShoot(int dmg)
	{
		GameObject bulletInstance = EquippedWeapon.GetComponent<IProjectileWeapon>().CastProjectile();
		bulletInstance.GetComponent<LaserBullet>().Damage = dmg;
		NetworkServer.Spawn(bulletInstance);
	}

	[Command]
	public void CmdShootAnim()
	{
		RpcShoot();
	}

	[ClientRpc]
	public void RpcShoot()
	{
		weaponPoint.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Shoot");
	}
}
