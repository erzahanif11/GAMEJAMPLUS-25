using System.Xml.Serialization;
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
    TestPlayer pScript;

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
        pScript = player.GetComponent<TestPlayer>();
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition((Vector2)transform.position + direction * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("You've hit a player!");
            pScript.minHealth(atkPower);
            Debug.Log("His health: " + pScript.getHealth());
        }
        Destroy(gameObject);
    }

}
