using UnityEngine;

public class bulletDestroy : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Pastikan tag ini sama dengan tag yang ada di prefab peluru")]
    public string targetTag = "Bullet"; 

    // Fungsi ini dipanggil otomatis ketika objek lain MASUK ke area collider trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Cek apakah objek yang masuk punya tag yang sesuai (Bullet)
        // Kita pakai CompareTag karena lebih hemat memori daripada other.tag == ...
        if (other.CompareTag(targetTag))
        {
            Destroy(other.gameObject);
        }
    }
}