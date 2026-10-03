using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; 

public class Player : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator anim;
    protected Collider2D col;
    protected SpriteRenderer sr;

    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int currentHealth;
    [SerializeField] private Material damageMaterial;
    public HealthBar healthBar;
    protected float damageFeedbackDuration = .1f;
    protected Coroutine damageFeedbackCoroutine;

    [SerializeField] protected float attackRadius;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask IsTarget;
    

    [SerializeField] protected float movespeed = 7.5f;
    [SerializeField] private float jump = 12f;
    [SerializeField] protected int playerDamage = 12;
    private float mobileInput = 0;

    private float cekground = 1.4f;
    private bool isGrounded;
    protected int facingDir = 1;
    protected bool kekanan = true;
    private float xInput;

    protected bool canMove = true;
    private bool canJump = true;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
    }

    private void Start() {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }


    protected virtual void Update() {
        HandleInput();
        Move();
        HandleAnimation();
        HandleFlip();
        HandleInfiniteJump();
    }

    private void HandleInput() {

        float keyboardInput = Input.GetAxisRaw("Horizontal");

        xInput = keyboardInput + mobileInput;

        xInput = Mathf.Clamp(xInput, -1f, 1f);

        bool kenaTombolUI = false;

        // Cek Versi HP (Touch)
        if (Input.touchCount > 0)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                kenaTombolUI = true;
            }
        }
        // Cek Versi PC (Mouse)
        else if (EventSystem.current.IsPointerOverGameObject())
        {
            kenaTombolUI = true;
        }

        if (kenaTombolUI) return;

        
    }

    public void SetMoveInput(float inputBaru)
   {
       mobileInput = inputBaru;
   }

    protected virtual void Move(){

        if(canMove) 
            rb.velocity = new Vector2(xInput * movespeed, rb.velocity.y);

        else 
            rb.velocity = new Vector2(0 , rb.velocity.y);
    }

    public void Jump(){
        if (isGrounded && canJump){
            rb.velocity = new Vector2(rb.velocity.x, jump);
        }
    }
    
    // Fungsi untuk char nya bisa munculin animasi jalan saat gerak
    protected void HandleAnimation() {
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);

    }
    
    // Fungsi untuk membuat char nya bisa nge flip ke arah yang berlawanan saat bergerak
    protected void HandleFlip() {
        if (rb.velocity.x > 0 && kekanan == false) 
            Flip();
        else if (rb.velocity.x < 0 && kekanan == true) 
            Flip();
    }

    protected void Flip() {
        transform.Rotate(0, 180, 0);
        kekanan = !kekanan;
        facingDir = facingDir * -1;
    }
    
    // Fungsi untuk ngebuat dia ga bisa loncat berkali kali di udara
    protected virtual void HandleInfiniteJump() {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, cekground, LayerMask.GetMask("Ground")); 
    }

    public void TriggerAttack()
    {
        // Di sini kita panggil fungsi asli yang protected itu
        AttackAnimation(); 
    }

    // Fungsi unyuk mengatur animasi attack
    protected virtual void AttackAnimation() {
       if (isGrounded) {
            anim.SetTrigger("attack");
            rb.velocity = new Vector2(0, rb.velocity.y);
       }
    }
    
    // Logika ketika player tidak bisa bergerak dan loncat ketika sedang melakukan attack
    public void SetCanMoveAndJump(bool enable) {
        canMove = enable;
        canJump = enable;
    }

    public void DamageTarget() {
        Collider2D[] enemycolliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, IsTarget);

        foreach (Collider2D target in enemycolliders){

            BossHealth boss = target.GetComponent<BossHealth>();
            if (boss != null)
            {
                boss.TakeDamage(playerDamage);
                continue; 
            }

            Player enemyScript = target.GetComponent<Player>(); 
            
            if(enemyScript != null) {
                enemyScript.TakeDamage(playerDamage); 
            }
        }
    }

    public virtual void TakeDamage(int enemydamage){
        currentHealth = currentHealth - enemydamage;

        healthBar.SetHealth(currentHealth);

        if (damageFeedbackCoroutine != null){
            StopCoroutine(damageFeedbackCoroutine);
        }
        StartCoroutine(DamageFeedbackCo());

        if (currentHealth <= 0){
            Die();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    protected virtual void Die(){
        anim.enabled = false;
        col.enabled = false;

        rb.gravityScale = 12;
        rb.velocity = new Vector2(rb.velocity.x, 15);

    }

    protected IEnumerator DamageFeedbackCo(){
        Material originalMat = sr.material;

        sr.material = damageMaterial;

        yield return new WaitForSeconds(damageFeedbackDuration);

        sr.material = originalMat;
    }

    // Fungsi untuk dia respawn 
    private void OnCollisionEnter2D(Collision2D target){
        if (target.gameObject.tag == "Deadly") {
            DieRespawn();
        }

        Enemy enemy = target.gameObject.GetComponent<Enemy>();
        if (enemy != null) {
            TakeDamage(enemy.damageOutput);
        }
    }

    private void DieRespawn(){
        Destroy (gameObject);
        //Application.LoadLevel(Application.loadedLevel); //untuk unity 4
        //SceneManager.LoadScene(1); //unity 5 pakai ini, tapi daftarin dulu scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //bisa tau posisi level sekarang
    }

    public void Heal(float healAmount){
        currentHealth += (int)healAmount;
        if (currentHealth > maxHealth){
            currentHealth = maxHealth;
        }
        healthBar.SetHealth(currentHealth);
    }
}