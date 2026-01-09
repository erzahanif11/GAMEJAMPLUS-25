using System.Xml.Serialization;
using Unity.Cinemachine;
using UnityEngine;

public class Enemy1A : MonoBehaviour
{
    public enum AtkDirection
    {
        Down,
        Right,
        Left
    }

    [Header("Setting")]
    [SerializeField] float lifeTime;
    [SerializeField] float speed;
    [SerializeField] int atkPower;
    [SerializeField] AtkDirection atkDirection;

    Vector2 direction;
    Rigidbody2D rb;
    GameObject player;
    PlayerStats pScript;
    Animator animator;
    CinemachineImpulseSource impulseSource;
    bool isHit = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if(atkDirection == AtkDirection.Down)
        {
            direction = Vector2.down;
        } else if(atkDirection == AtkDirection.Left)
        {
            direction = Vector2.left;
        } else if(atkDirection == AtkDirection.Right)
        {
            direction = Vector2.right;
        }

        player = GameObject.FindGameObjectWithTag("Player");
        pScript = player.GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isHit)
        rb.MovePosition((Vector2)transform.position + direction * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        isHit = true;
        animator.SetTrigger("isHit");
        
        if (collision.gameObject.CompareTag("Player"))
        {
            CameraShakeManager.instance.CameraShake(impulseSource);
            AudioManager.instance.PlaySFX(AudioManager.instance.attacked);
            Debug.Log("You've hit a player!");
            pScript.TakeDamage();
            Debug.Log("His health: " + pScript.lives);
        }
        if (collision.gameObject.CompareTag("Lava"))
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.lavaHiss);
        }
        if (collision.gameObject.CompareTag("Block"))
        {
            CameraShakeManager.instance.CameraShake(impulseSource);
            AudioManager.instance.PlaySFX(AudioManager.instance.attacked);
        }
    }

}
