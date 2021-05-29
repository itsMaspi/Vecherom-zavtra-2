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
		//var itemSlug = GetComponent<InventoryUI>().inventoryPanel.Find("Inventory_Details").GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text;
		var itemSlug = GetComponent<InventoryUI>().inventoryPanel.Find("Inventory_Details").GetComponent<InventoryUIDetails>().GetItem().ObjectSlug;
		CmdDropItem(itemSlug, transform.position);
		GetComponent<InventoryController>().RemoveItem(itemSlug);
		GetComponent<InventoryUI>().inventoryPanel.Find("Inventory_Details").GetComponent<InventoryUIDetails>().DestroySelectedItem();
	}

	[Command]
	public void CmdDropItem(string itemSlug, Vector3 position)
    {
		Vector3 pos = new Vector3(position.x + 1f, position.y);
		GameObject weapon = Instantiate(Resources.Load<GameObject>($"Drops/{itemSlug}_drop"), pos, transform.rotation);
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
		EquippedWeapon.GetComponent<IWeapon>().Stats = GetComponent<ItemDatabase>().GetItem(itemToEquip).Stats;

		if (isLocalPlayer)
		{
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


		if (value.isPressed)
		{
			GetComponent<Animator>().SetBool("isShooting", true);
		}
	}

	[Command]
	public void CmdShoot(int dmg, int speed, int range)
	{
		GameObject bulletInstance = EquippedWeapon.GetComponent<IProjectileWeapon>().CastProjectile();
		bulletInstance.GetComponent<LaserBullet>().Damage = dmg;
		bulletInstance.GetComponent<LaserBullet>().Speed = speed;
		bulletInstance.GetComponent<LaserBullet>().Range = range;
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
