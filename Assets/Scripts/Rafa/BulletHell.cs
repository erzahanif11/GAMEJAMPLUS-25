using UnityEngine;

public class BulletHell : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float fireRate = 0.1f;
    public float rotationSpeed = 10f;
    public int numberOfProjectiles = 1;
    private float currentAngle = 0f;
    public float bulletspeed=5f;

    void Start()
    {
        // Memanggil fungsi tembak secara berulang
        InvokeRepeating("Fire", 0f, fireRate);
    }

    void Fire()
    {
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            // Menghitung rotasi untuk peluru ini
            float angleStep = 360f / numberOfProjectiles;
            float angle = currentAngle + (i * angleStep);

            // Mengonversi sudut ke arah vektor
            float x = Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 direction = new Vector3(x, y, 0).normalized;

            // Membuat peluru
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            
            // Memberikan kecepatan ke peluru (asumsi peluru punya script gerak)
            bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * bulletspeed;
        }

        // Memutar sudut spawner untuk tembakan berikutnya (efek spiral)
        currentAngle += rotationSpeed;
    }
}