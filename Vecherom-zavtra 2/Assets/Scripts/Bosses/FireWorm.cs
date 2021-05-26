using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorm : NetworkBehaviour, IEnemy
{

    [Header("Stats")]
    public int maxHealth;
    [SerializeField] float attackDistance = 15f;
    [SerializeField] float attackCooldown = 1f;
    float attackTime = 0f;
    //[SerializeField] private LayerMask whatCanDamage;

    [Space]
    [Header("Other")]

    [SyncVar(hook = nameof(OnChangedHealth))]
    [HideInInspector] public int currentHealth;

    [HideInInspector] public Animator animator;
    public TMPro.TextMeshProUGUI healthBar;

    private CharacterStats characterStats;
    [HideInInspector] public EnemyController2D controller;

    [Server]
    public void CmdTakeDamage(int amount)
    {
        currentHealth -= amount;
        animator.SetTrigger("Hit");
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animator.SetTrigger("Death");
        }
    }

    [Server]
    public void CmdDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    public void PerformAttack()
    {
        throw new System.NotImplementedException();
    }

    public void Awake()
    {
        characterStats = new CharacterStats(15, 2, 10, 0, 0, 0);
        currentHealth = maxHealth;
        controller = GetComponent<EnemyController2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isServer) return;
        TryAttack();
    }

    public void TryAttack()
    {
        /*
        if (controller.targetPlayer != null)
        {
            if (Vector3.Distance(controller.targetPlayer.position, transform.position) <= attackDistance)
            {
                animator.SetBool("isAttacking", true);
            }
            else
            {
                animator.SetBool("isAttacking", false);
            }
        }*/
    }

    public void OnChangedHealth(int oldHealth, int newHealth)
    {
        healthBar.text = newHealth.ToString();
    }
}
