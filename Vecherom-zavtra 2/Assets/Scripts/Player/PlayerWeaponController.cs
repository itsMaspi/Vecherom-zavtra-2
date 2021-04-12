using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public GameObject weaponPoint;
	public GameObject EquippedWeapon { get; set; }

	IWeapon equippedWeapon;
	CharacterStats characterStats;

	void Start()
	{
		characterStats = GetComponent<CharacterStats>();
	}

	public void EquipWeapon(Item itemToEquip) // REFACTOR: EquipItem()
	{
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
		Debug.Log(characterStats.stats[1].GetCalculatedStatValue());
	}

	public void PerformWeaponAttack()
	{
		equippedWeapon.PerformAttack();
	}
}
