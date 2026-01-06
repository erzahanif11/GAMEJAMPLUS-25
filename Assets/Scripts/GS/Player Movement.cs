using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private string attackAnimationName = "Attack";
    private string velocityYParameter = "VelocityY";
    private string velocityXParameter = "VelocityX";
    private string isGroundedParameter = "IsGrounded";
    private string dashParameter = "Dash";
    private string groundedRightParameter = "GroundedRight";
    private string jumpParameter = "Jump";
    
    PlayerStats stats;
    Grappler grappler;
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public Transform groundCheck;
    public Transform groundCheckLeft;
    public Transform groundCheckRight;
    public float groundCheckRadius = 0.2f;
    public float groundCheckRadiusHorizontal = 0.1f;

    public LayerMask groundLayer;
    Rigidbody2D rb;
    float moveInput;
    [SerializeField] bool isGrounded;
    bool jumpedDouble;

    public Transform wallCheck;
    public LayerMask wallLayer;
    bool isSliding;
    float slidingSpeed;

    bool isWallJumping;
    float wallJumpDirection;
    public float wallJumpTime = 0.5f;
    public Vector2 wallJumpPower = new Vector2(5f, 10f);

    public bool isGrappling;
    public bool isGhostMode;

    [SerializeField] TrailRenderer trail;
    public bool canDash;
    bool isDashing;
    public float dashingForce = 5f;
    float dashingTime = 0.25f;
    public float dashingCooldown = 0.5f;

    float originalSpeed;
    float originalJump;
    Coroutine speedEffectCoroutine;
   public Animator animator;

    public float footdistanceonground=0.5f;
    public float footdistancemidair=0.5f;

    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayer;
    private bool canAttack = true;

    // public GameObject OnGroundJumpEffect; 
    // public GameObject MidAirJumpEffect;


    private void Awake()
    {
        groundLayer = LayerMask.GetMask("Ground");
        stats = FindAnyObjectByType<PlayerStats>();
        grappler = GetComponent<Grappler>();
       // audioManager = GameObject.FindGameObjectWithTag("Audio Manager").GetComponent<AudioManager>();
    }

    void Start()
    {
        if (animator==null)
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        canDash = true;
        slidingSpeed = rb.gravityScale / 2f;

        originalSpeed = moveSpeed;
        originalJump = jumpForce;

        
      //  animator = GetComponent<Animator>();

    }
    bool groundedCenter;
    bool groundedLeft;
    bool groundedRight;
    void Update()
    {
        SetAnimatorParameters();
        if (isDashing)
        {
            
            return;
        }

        isGrappling = grappler.isGrappling;

        // Ground check di tiga titik
         groundedCenter = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius*1.25f, groundLayer);
         groundedLeft = Physics2D.OverlapCircle(groundCheckLeft.position, groundCheckRadiusHorizontal, groundLayer);
         groundedRight = Physics2D.OverlapCircle(groundCheckRight.position, groundCheckRadiusHorizontal, groundLayer);

        isGrounded = groundedCenter || groundedLeft;

        if (isGrounded || groundedRight)
        {
            jumpedDouble = false;
        }
        // Input gerak kiri-kanan
        if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1f;
            Flip();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1f;
            Flip();
        }
        else moveInput = 0f;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            if (isGrounded)
            {
                Jump();
            }
            else if (!isGrounded && !jumpedDouble && !isGhostMode && !isSliding)
            {
                Jump();
                jumpedDouble = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isGhostMode)
        {
            if (stats.UseStamina(4f)){
                StartCoroutine(Dash());
            }
        }

        isSliding = Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
        wallSlide();

        if (isSliding && !isGrounded && ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))))
        {
            WallJump();
        }

        if (isGrappling){
            rb.gravityScale = 1f;
        }else {
            rb.gravityScale = 2f;
        }

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            canAttack = false;
            animator.SetTrigger(attackAnimationName);
            StartCoroutine(AttackCooldown());
        }
    }

    void FixedUpdate()
    {
        

        // animator.SetFloat("VelocityX", Mathf.Abs(rb.linearVelocity.x));
        // animator.SetFloat("VelocityY", rb.linearVelocity.y);
        // animator.SetBool("isGrounded", isGrounded);
        // animator.SetBool("isSliding", (isSliding&&!isGrounded));
        // animator.SetBool("hug wall", groundedLeft||groundedRight);


        // animator.SetLayerWeight(0, isGhostMode ? 0f : 1f);
        // animator.SetLayerWeight(1, isGhostMode ? 1f : 0f);

        if (isDashing)
        {
            return;
        }

        if (!isWallJumping && !isGrappling)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    void SetAnimatorParameters(){
        animator.SetFloat(velocityXParameter, Mathf.Abs(rb.linearVelocity.x));
        animator.SetFloat(velocityYParameter, rb.linearVelocity.y);
        animator.SetBool(isGroundedParameter, isGrounded);
        animator.SetBool(groundedRightParameter, groundedRight);
    }

    void Jump()
    {
        animator.SetTrigger(jumpParameter);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
//        audioManager.PlaySFX(audioManager.jump);
        if (isGrounded)
        {
            Vector3 spawnPos = transform.position + new Vector3(0, footdistanceonground, 0); // sesuaikan posisi
       //     Instantiate(OnGroundJumpEffect, spawnPos, Quaternion.identity);
        }
        else if (!isGrounded)
        {
            Vector3 spawnPos = transform.position + new Vector3(0, footdistancemidair, 0); // sesuaikan posisi
    //        Instantiate(MidAirJumpEffect, spawnPos, Quaternion.identity);
        }
        
    }

    void wallSlide()
    {
        if (isSliding && !isGrounded && !isWallJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -slidingSpeed, float.MaxValue));
        }
    }

    void WallJump()
    {
        isWallJumping = true;
        wallJumpDirection = -transform.localScale.x;
        rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);

        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        Invoke(nameof(StopWallJump), wallJumpTime);
    }

    void StopWallJump()
    {
        isWallJumping = false;
    }

    void Flip()
    {
        float scaleX = Mathf.Abs(transform.localScale.x);
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-scaleX, transform.localScale.y, transform.localScale.z);
        }
    }

    IEnumerator Dash()
    {
        // canDash = false;
        isDashing = true;
        animator.SetTrigger(dashParameter);
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashingForce, 0f);
        AudioManager.instance.PlaySFX(AudioManager.instance.dash);
        trail.emitting = true;
        Debug.Log("Dash start");
        yield return new WaitForSeconds(dashingTime);
        Debug.Log("Dash ended");
        trail.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        // canDash = true;
    }

    void Claw(){
        if (canAttack)
        {
            Debug.Log("Claw");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
            foreach(Collider2D enemy in hitEnemies){
                Debug.Log("Enemy hit");
                Destroy(enemy.gameObject);
            }
        }else{
            Debug.Log("Attack on cooldown");
        }
    }

    IEnumerator AttackCooldown(){
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // public void ApplySpeedBoost(float boostMultiplier, float boostDuration, float slowMultiplier, float slowDuration)
    // {
    //     audioManager.PlaySFX(audioManager.powerUp);
    //     if (speedEffectCoroutine != null)
    //     {
    //         StopCoroutine(speedEffectCoroutine);
    //     }
    //     speedEffectCoroutine = StartCoroutine(speedEffect(boostMultiplier, boostDuration, slowMultiplier, slowDuration));
    // }

    // IEnumerator speedEffect(float boostMultiplier, float boostDuration, float slowMultiplier, float slowDuration)
    // {
    //     moveSpeed = originalSpeed * boostMultiplier;
    //     jumpForce = originalJump * boostMultiplier;
    //     yield return new WaitForSeconds(boostDuration);

    //     moveSpeed = originalSpeed * slowMultiplier;
    //     jumpForce = originalJump * slowMultiplier;
    //     yield return new WaitForSeconds(slowDuration);

    //     moveSpeed = originalSpeed;
    //     jumpForce = originalJump;
    //     speedEffectCoroutine = null;
    // }

    // public void ActivateGhostMode(float duration)
    // {
    //     audioManager.PlaySFX(audioManager.powerUp);
    //     StartCoroutine(GhostModeCoroutine(duration));
    // }

    // IEnumerator GhostModeCoroutine(float duration)
    // {
    //     isGhostMode = true;
    //     Debug.Log("Ghost aktif");
    //     yield return new WaitForSeconds(duration);
    //     isGhostMode = false;
    //     Debug.Log("Ghost non aktif");
    // }
}