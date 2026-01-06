using UnityEngine;

public class Enemy1B : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] float lifeTime;
    [SerializeField] float speed;
    [SerializeField] float travelingTime;
    [SerializeField] float waitingTime;
    [SerializeField] float backTime;
    [SerializeField] int atkPower;

    Rigidbody2D rb;
    Animator animator;
    GameObject player;
    PlayerStats pScript;
    Vector2 targetPos;
    bool hasTarget = false;
    bool isActive = true;
    bool isHit = false;
    bool isFlipped;
    SpriteRenderer eSprite;
    float travel;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        eSprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) pScript = player.GetComponent<PlayerStats>();
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
        travel = travelingTime;
        int flipInt = Random.Range(0,2);
        if (flipInt == 1)
        {
            isFlipped = true;
            eSprite.flipX = true;
            Debug.Log("Flipped Enemy!");
        }
        else
        {
            isFlipped = false;
            eSprite.flipX = false;
            Debug.Log("Non-Flipped Enemy!");
        }
        
        rb.gravityScale = 0; 
    }

    void FixedUpdate()
    {
        if (player != null && isActive)
        {
            StartBehaviour();
        }
    }

    void StartBehaviour()
    {
        if (travel > 0 && !isHit)
        {
            rb.MovePosition(rb.position + Vector2.down * speed * Time.fixedDeltaTime);
            travel -= Time.fixedDeltaTime;
        }
        else if (waitingTime > 0 && !isHit)
        {
            waitingTime -= Time.fixedDeltaTime;
            rb.linearVelocity = Vector2.zero; 
        }
        else if (backTime > 0 && !isHit)
        {
            if (!isFlipped)
            {
                rb.MovePosition(rb.position + Vector2.left * speed * Time.fixedDeltaTime);
            } else{
                rb.MovePosition(rb.position + Vector2.right * speed * Time.fixedDeltaTime);
            }
            backTime -= Time.fixedDeltaTime;
        }
        else
        {
            if (!hasTarget && !isHit)
            {
                animator.SetTrigger("isAttack");
                targetPos = player.transform.position;
                Vector2 direction = (targetPos - (Vector2)transform.position).normalized;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

                rb.linearVelocity = direction * (speed + 5);
                hasTarget = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        isHit = true;
        animator.SetTrigger("isHit");
        if (!isActive) return;

        isActive = false;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false; 

        if (collision.gameObject.CompareTag("Player"))
        {
            if (pScript != null)
            {
                pScript.TakeDamage();
                Debug.Log("Health Player: " + pScript.lives);
            }
        }
    }
}