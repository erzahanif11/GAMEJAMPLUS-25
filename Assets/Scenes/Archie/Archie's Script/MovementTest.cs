using System;
using Unity.VisualScripting;
using UnityEngine;

public class MovementTest : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] float speed;
    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal")*speed, 0f);
    }
}
