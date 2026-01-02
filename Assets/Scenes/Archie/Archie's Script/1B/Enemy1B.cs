using Unity.VisualScripting;
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
    GameObject player;
    TestPlayer pScript;
    Vector2 targetPos;
    bool hasTarget = false;
    bool isActive = true;
    bool isFlipped;
    SpriteRenderer eSprite;
    float travel;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        eSprite = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) pScript = player.GetComponent<TestPlayer>();
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
        travel = travelingTime;
        int flipInt = Random.Range(0,2);
        switch (flipInt)
        {
            case 1:
                isFlipped = true;
                eSprite.flipX = true;
                Debug.Log("Flipped Enemy!");
                break;
            case 2:
                isFlipped = false;
                eSprite.flipX = false;
                Debug.Log("Non-Flipped Enemy!");
                break;
        }
        
        // TIPS: Pastikan Gravity Scale di Rigidbody2D adalah 0 
        // agar tidak beradu dengan MovePosition
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
        if (travel > 0)
        {
            rb.MovePosition(rb.position + Vector2.down * speed * Time.fixedDeltaTime);
            travel -= Time.fixedDeltaTime;
        }
        else if (waitingTime > 0)
        {
            waitingTime -= Time.fixedDeltaTime;
            rb.linearVelocity = Vector2.zero; // Rem total agar tidak geser
        }
        else if (backTime > 0)
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
            // FASE DASH: Menggunakan Velocity agar Smooth saat benturan
            if (!hasTarget)
            {
                targetPos = player.transform.position;
                Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
                
                // Set kecepatan sekali saja (seperti peluru)
                rb.linearVelocity = direction * (speed + 5);
                hasTarget = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isActive) return;

        isActive = false;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false; // Mematikan fisika agar tidak jitter saat proses Destroy

        if (collision.gameObject.CompareTag("Player"))
        {
            if (pScript != null)
            {
                pScript.minHealth(atkPower);
                Debug.Log("Health Player: " + pScript.getHealth());
            }
        }

        Destroy(gameObject);
    }
}