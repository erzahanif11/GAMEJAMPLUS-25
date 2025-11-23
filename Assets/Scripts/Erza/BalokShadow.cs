using System.Runtime.InteropServices;
using UnityEngine;

public class BalokShadow : MonoBehaviour
{
    public LayerMask groundLayer;
    public float maxDistance = 20f;
    public float minScaleY = 0.1f;
    public float maxScaleY = 1f;

    public Rigidbody2D rb;
    private SpriteRenderer srMain;
    private GameObject shadowObject;
    private SpriteRenderer srShadow;

    private void Awake()
    {
        groundLayer = LayerMask.GetMask("Ground");
    }
    void Start()
    {
        srMain = GetComponent<SpriteRenderer>();
        rb = GetComponentInParent<Rigidbody2D>();

        // === AUTO BUAT GAMEOBJECT SHADOW ===
        shadowObject = new GameObject("Shadow_" + gameObject.name);
        srShadow = shadowObject.AddComponent<SpriteRenderer>();
        shadowObject.transform.parent = transform.parent; // agar di hierarchy tidak berantakan
        
        // Ambil sprite balok
        srShadow.sprite = srMain.sprite;

        // Warna bayangan
        srShadow.color = new Color(0, 0, 0, 0.35f);

        // Layer & order agar muncul di belakang objek
        srShadow.sortingLayerID = srMain.sortingLayerID;
        srShadow.sortingOrder = srMain.sortingOrder - 1; // di belakang balok

        shadowObject.SetActive(false);
    }

    void Update()
    {
        float halfHeight = srMain.bounds.size.y / 2f;
        Vector2 origin = (Vector2)transform.position + Vector2.down * (halfHeight + 0.15f);

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, maxDistance, groundLayer);
    Debug.DrawRay(transform.position, Vector2.down * maxDistance, Color.green);

    if (hit.collider != null && rb.linearVelocity.y < -0.1f)
    {
        shadowObject.SetActive(true);

        float jarak = hit.distance;
        float tinggiSprite = srShadow.sprite.bounds.size.y;
        float scaleY = jarak / tinggiSprite;

        // Terapkan scale sesuai jarak balok ke tanah
        shadowObject.transform.localScale = new Vector3(
            transform.localScale.x + 0.05f,
            scaleY,
            transform.localScale.z
        );

        // Tempatkan bayangan agar memanjang ke bawah
        shadowObject.transform.position = new Vector2(
            transform.position.x,
            hit.point.y + (srShadow.sprite.bounds.size.y * scaleY) / 2f
        );
    }
    else
    {
        shadowObject.SetActive(false);
    }
    }
}