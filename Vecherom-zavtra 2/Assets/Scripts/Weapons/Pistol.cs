using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour, IWeapon
{
	private Animator animator;
	public List<BaseStat> Stats { get; set; }

	void Start()
	{
		animator = GetComponent<Animator>();
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
}
