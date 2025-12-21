using UnityEngine;

public class LavaEater : MonoBehaviour
{
     float sinkSpeed = 0.1F;

    private void OnTriggerStay2D(Collider2D other)
    {
        //get parent object
        
        if (other.CompareTag("Block"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            if (rb.bodyType != RigidbodyType2D.Kinematic)
            {
                //+5 poin
               rb.bodyType = RigidbodyType2D.Kinematic;
               rb.linearVelocity = new Vector2(0, 0);
            }

            int weight=other.GetComponent<Weightcount>().weightCount;
            if (weight ==0)
            weight=1;
           other.transform.position -= new Vector3(0, weight*sinkSpeed * Time.deltaTime, 0);
           Debug.Log("" + weight * sinkSpeed + "");
        }
        else  if (other.CompareTag("Player") && GameManager.instance != null)
        {
                GameManager.instance.Death();
        }


    }
}
