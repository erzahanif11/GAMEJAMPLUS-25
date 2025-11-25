using Unity.VisualScripting;
using UnityEngine;

public class DeleteBlocks : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnTriggerEnter2D(Collider2D other)
    {
        //kill parent if parent is tag block
         GameObject parentObject = other.transform.parent != null ? other.transform.parent.gameObject : other.gameObject;
        if(parentObject.CompareTag("Block"))
        {
            Destroy(parentObject);
        }
    
    }   
}
