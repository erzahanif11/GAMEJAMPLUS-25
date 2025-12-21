using System.Collections;
using UnityEngine;

public class KillZoneScript : MonoBehaviour
{
    public Collider2D killZoneCollider;
    public Weightcount stacks;
    void Awake()
    {

        stacks = gameObject.GetComponentInParent<Weightcount>();    
        if (killZoneCollider == null)
        killZoneCollider = GetComponent<Collider2D>();
    
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player")&& GameManager.instance != null && gameObject.GetComponentInParent<Rigidbody2D>().linearVelocityY < -0.5f)
        {
            GameManager.instance.Death();
        }

        else if(other.CompareTag("Block")){
                if ( stacks != null && !stacks.belowBlocks.Contains(other.gameObject))
                {

                    stacks.belowBlocks.Add(other.gameObject);   
                    stacks.dropWeight(0);            
                }
            
            }
        
        }

}
