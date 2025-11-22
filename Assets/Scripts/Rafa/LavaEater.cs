using UnityEngine;

public class LavaEater : MonoBehaviour
{
    public float sinkSpeed = 5e-16f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Block"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // disable physics
               rb.bodyType = RigidbodyType2D.Kinematic;
               rb.linearVelocity = new Vector2(0, 0);
            }

           other.transform.position -= new Vector3(0, sinkSpeed * Time.deltaTime, 0);
        }
        else  if (other.CompareTag("Player") && GameManager.instance != null)
        {
                GameManager.instance.Death();
        }




    }
}
