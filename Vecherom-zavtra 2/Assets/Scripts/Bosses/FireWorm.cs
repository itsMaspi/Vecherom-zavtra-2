using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorm : NetworkBehaviour, IEnemy
{
    [SyncVar(hook = nameof(OnChangedHealth))]
    [HideInInspector] public int currentHealth;

    [Header("Stats")]
    public int maxHealth;
    [SerializeField] float attackDistance = 45f;
    [SerializeField] float passiveAttackRange = 10f;
    public float attackCooldown = 5f;
    public float attackSpeed = 100f;
    public int damage = 40;
    float attackTime = 0f;
    float passiveAttackTime = 0f;
    float passiveAttackCooldown = 0.5f;

    [Header("Attack")]
    public string projectileSlug = null;
    [SerializeField] Transform firePoint = null;

    [SerializeField] private LayerMask whatCanDamage;

    [Header("Other stuff")]
    [HideInInspector] public Animator animator;
    public TMPro.TextMeshProUGUI healthBar;

    private CharacterStats characterStats;
    [HideInInspector] public EnemyController2D controller;


    public void Awake()
    {
        characterStats = new CharacterStats(10, 2, 10, 0, 0, 0);
        currentHealth = maxHealth;
        controller = GetComponent<EnemyController2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        attackTime += Time.deltaTime;
        passiveAttackTime += Time.deltaTime;
        TryAttack();
        if (passiveAttackTime>=passiveAttackCooldown)
        {
            PassiveAttack();
        }
    }

    //Called by animator trigger 'Attack'
    public void PerformAttack()
    {
        Vector3 force = controller.targetPlayer.position - firePoint.position;

        CmdAttack1(projectileSlug, force.normalized, Vector3.SignedAngle(firePoint.position, controller.targetPlayer.position, transform.right));
    }

    [Command(requiresAuthority = false)]
    public void CmdAttack1(string projectileSlug, Vector3 force, float angle)
    {
        Debug.Log($"Force: {force}");
        Fireball fireball = Instantiate(Resources.Load<Fireball>($"Bosses/Projectiles/{projectileSlug}"), firePoint.position, firePoint.rotation);
        fireball.Force = force;
        fireball.Speed = attackSpeed;
        fireball.Range = 10f;
        fireball.Damage = damage;
        GameObject fireballG = fireball.gameObject;

        //Flip sprite
        var theScale = fireballG.transform.localScale;
        if (force.x < 0)
        {
            theScale.x *= Mathf.FloorToInt(force.x);
        } else
        {
            theScale.x *= Mathf.CeilToInt(force.x);
        }

        fireballG.transform.localScale = theScale;
        //Debug.Log($"Angle{Quaternion.LookRotation(force, force)}");
        //fireballG.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);



        NetworkServer.Spawn(fireballG);
    }

    public void TryAttack()
    {
        if (attackTime < attackCooldown) return;
        if (controller.targetPlayer != null)
        {
            if (Vector3.Distance(controller.targetPlayer.position, transform.position) <= attackDistance)
            {
                animator.SetTrigger("Attack");
                attackTime = 0;
            }
        }
    }

    private void PassiveAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, passiveAttackRange, whatCanDamage);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].tag == "Player")
            {
                colliders[i].GetComponent<Player>().TakeDamage(characterStats.GetStat(BaseStat.BaseStatType.Damage).GetCalculatedStatValue());
                Debug.Log($"DmgDealt = {characterStats.GetStat(BaseStat.BaseStatType.Damage).GetCalculatedStatValue()}");
                passiveAttackTime = 0;
            }
        }
    }

    [Command(requiresAuthority =false)]
    public void CmdStopMove()
    {
        controller.canMove = false;
    }

    [Command(requiresAuthority = false)]
    public void CmdStartMove()
    {
        controller.canMove = true;
    }

    [Server]
    public void CmdTakeDamage(int amount)
    {
        currentHealth -= amount;
        animator.SetTrigger("Hit");
        if (currentHealth <= maxHealth/2)
        {
            animator.SetBool("isEnraged", true);
        } 
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animator.SetBool("isDead", true);
        }
    }

    [Server]
    public void CmdDie()
    {
        CmdSpawnTrigger();
    }

    [Command(requiresAuthority = false)]
    public void CmdSpawnTrigger()
    {
        GameObject trigger = (GameObject)Instantiate(Resources.Load("Triggers/CameraZoomInTrigger"), transform.position, Quaternion.identity);
        
        NetworkServer.Spawn(trigger);
        Debug.Log($"Spawned camera zoom in trigger: {trigger}");
    }

    [Command(requiresAuthority = false)]
    public void CmdCallStop()
    {
        RPCStop();
    }

    [ClientRpc]
    public void RPCStop()
    {

        GetComponent<EnemyController2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<FireWorm>().enabled = false;
        GetComponent<FireWorm>().healthBar.enabled = false;
        var colliders = GetComponents<CapsuleCollider2D>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
    }


    public void OnChangedHealth(int oldHealth, int newHealth)
    {
        healthBar.text = newHealth.ToString();
    }
}
