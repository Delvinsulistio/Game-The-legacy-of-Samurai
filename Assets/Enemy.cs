using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Player
{   [Header("Status Musuh")]
    [SerializeField] public int damageOutput = 10;

    [Header("Patrol Settings")]
    [SerializeField] private Transform ledgeCheckPoint;
    [SerializeField] private float ledgeCheckDistance = 1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Back Detection")]
    [SerializeField] private Transform backCheckPoint;
    [SerializeField] private float backCheckRadius = 0.8f;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField]private int maxHealthEnemy;


    private void Start()
    {
        facingDir = -1;
        kekanan = false;

        currentHealth = maxHealthEnemy;
        healthBar.SetMaxHealth(maxHealthEnemy);


    }

    private bool playerDetected;

    protected override void Update()
    {
        PatrolLedgeCheck();
        Move();
        HandleAnimation();
        HandleInfiniteJump();
        AttackAnimation();
        CheckBehind(); 
    }

    private void PatrolLedgeCheck()
    {
      
        if (!canMove) return;

        RaycastHit2D hitInfo = Physics2D.Raycast(ledgeCheckPoint.position, Vector2.down, ledgeCheckDistance, groundLayer);

        // Logika untuk kolo ga ada layer maka puter badan
        if (hitInfo.collider == null)
        {
          
            Flip();
        }

    }
    
    protected override void Move()
    {
        if (canMove)
            rb.velocity = new Vector2(facingDir * movespeed, rb.velocity.y);

        else
            rb.velocity = new Vector2(0, rb.velocity.y);
    }

    protected override void AttackAnimation()
    {
        if (playerDetected)
        {
            anim.SetTrigger("attack");
        }
    }

    public override void TakeDamage(int damageReceived)
    {
        currentHealth = currentHealth - damageReceived;

        if (currentHealth <= 0){
            Die();
            
        }

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    
    }

    public void DealDamageToPlayer()
    {
        Collider2D[] players = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, playerLayer);

        foreach (Collider2D playerHit in players)
        {
            // Kita cari script 'Player' di object yang kena pukul
            Player scriptPlayer = playerHit.GetComponent<Player>();

            if (scriptPlayer != null)
            {
                // Masukkan damageOutput (10 atau 20) ke fungsi TakeDamage milik Player
                scriptPlayer.TakeDamage(damageOutput);
            }
        }
    }

    protected override void HandleInfiniteJump()
    {
        base.HandleInfiniteJump();
        playerDetected = Physics2D.OverlapCircle(attackPoint.position, attackRadius, IsTarget) != null;
    }

    private void CheckBehind()
    {
        if (!canMove || backCheckPoint == null) return;
        
        Collider2D playerDetected = Physics2D.OverlapCircle(backCheckPoint.position, backCheckRadius, playerLayer);

       if (playerDetected != null)
        {
            Flip();
        }
    }

    public void FinishAttack()
    {
        canMove = true;
        
        // Reset trigger attack biar ga nyangkut (
        anim.ResetTrigger("attack");

    }

    private void OnDrawGizmos()
    {
        if (ledgeCheckPoint != null)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(ledgeCheckPoint.position, ledgeCheckPoint.position + Vector3.down * ledgeCheckDistance);
        }
    }
}