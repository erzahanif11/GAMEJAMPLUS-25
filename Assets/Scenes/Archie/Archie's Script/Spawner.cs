using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float spawnCD;
    [SerializeField] SpawnDir direction;

    BoxCollider2D bCollider;
    float spawnTime;
    int n;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        Vector2 spawnPos = Vector2.zero;
        Bounds cBound = bCollider.bounds;

        switch(direction)
        {
            case SpawnDir.Down:
                spawnPos.x = Random.Range(cBound.min.x, cBound.max.x);
                spawnPos.y = cBound.min.y;
                break;
            case SpawnDir.Left:
                spawnPos.x = cBound.min.x;
                spawnPos.y = Random.Range(cBound.min.y, cBound.max.y);
                break;
            case SpawnDir.Right:
                spawnPos.x = cBound.max.x;
                spawnPos.y = Random.Range(cBound.min.y, cBound.max.y);
                break;
        }

        GameObject enemey = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        n++;
        Debug.Log("Spawn ke-" + n + "Berhasil");
    }

    enum SpawnDir
    {
        Down,
        Left,
        Right
    }
}
