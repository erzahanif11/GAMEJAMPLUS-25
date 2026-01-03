using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float spawnCD;

    [Space]

    [Header("Attack Type Settings")]
    [SerializeField] int normalRate;
    [SerializeField] SpawnDir direction;

    GameObject playerObj;
    BoxCollider2D bCollider;
    float spawnTime;
    int n;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        bCollider = GetComponent<BoxCollider2D>();
        spawnTime = spawnCD;
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnTime > 0) spawnTime -= Time.deltaTime;

        if(spawnTime <= 0)
        {
            spawn();
            spawnTime = spawnCD;
        }
    }

    void spawn()
    {
        int typeAtk = Random.Range(0,100);
        Debug.Log("Type atk is: " + typeAtk);
        Vector2 spawnPos = Vector2.zero;
        Bounds cBound = bCollider.bounds;

        switch(direction)
        {
            case SpawnDir.Down:
                if(typeAtk < normalRate)
                {
                    spawnPos.x = Random.Range(cBound.min.x, cBound.max.x);
                    spawnPos.y = cBound.min.y;
                } else 
                {
                    spawnPos.x = playerObj.transform.position.x;
                    spawnPos.y = cBound.min.y;
                }
                break;
            case SpawnDir.Left:
                if(typeAtk < normalRate)
                {
                    spawnPos.x = cBound.min.x;
                    spawnPos.y = Random.Range(cBound.min.y, cBound.max.y);
                } else 
                {
                    spawnPos.x = cBound.min.x;
                    spawnPos.y = playerObj.transform.position.y;
                }
                break;
            case SpawnDir.Right:
                if(typeAtk < normalRate)
                {
                    spawnPos.x = cBound.max.x;
                    spawnPos.y = Random.Range(cBound.min.y, cBound.max.y);
                } else
                {
                    spawnPos.x = cBound.max.x;
                    spawnPos.y = playerObj.transform.position.y;
                }
                break;
        }

        GameObject sEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        sEnemy.transform.rotation = enemyPrefab.transform.rotation;
        n++;
        Debug.Log("Enemy number: " + n);
    }

    enum SpawnDir
    {
        Down,
        Left,
        Right
    }
}
