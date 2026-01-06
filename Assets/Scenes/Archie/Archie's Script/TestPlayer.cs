using Unity.VisualScripting;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [Header("Player health count")]
    [SerializeField] int health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int minHealth(int x)
    {
        return health -= x;
    }

    public int getHealth()
    {
        return health;
    }
}
