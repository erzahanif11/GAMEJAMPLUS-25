using UnityEngine;

public class LavaEater : MonoBehaviour
{
    public float sinkSpeed = 5e-16f;

    private void OnTriggerStay2D(Collider2D other)
    {
        //get parent object
        GameObject parentObject = other.transform.parent != null ? other.transform.parent.gameObject : other.gameObject;    
        
        if (parentObject.CompareTag("Block"))
        {
            Rigidbody2D rb = parentObject.GetComponent<Rigidbody2D>();

            bool flag= false;
            if (rb != null && !flag)
            {
                flag= true;
                // disable physics
               rb.bodyType = RigidbodyType2D.Kinematic;
               rb.linearVelocity = new Vector2(0, 0);
            }

           parentObject.transform.position -= new Vector3(0, sinkSpeed * Time.deltaTime, 0);
        }
        else  if (other.CompareTag("Player") && GameManager.instance != null)
        {
                GameManager.instance.Death();
        }




    }
}
