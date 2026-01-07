using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public static BossTrigger instance;
    bool isDeath = false;
    void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool bossDeath()
    {
        return isDeath = true;
    }
}
