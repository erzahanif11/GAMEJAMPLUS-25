using System.Collections.Generic;
using UnityEngine;

public class BlockSpawn : MonoBehaviour
{


    [SerializeField] ListofBlocks normal;

    public static BlockSpawn instance;
    public float spawnInterval = 1.5f;

     private
    void Awake()
    {
        instance = this;
    }


float curtime=0;
    void Update()
    {
        curtime+=Time.deltaTime;
        if (curtime >= spawnInterval)
        {
            SpawnBlock();
            curtime=0;
        }
    }

    void SpawnBlock()
    {
        GameObject blockToSpawn = normal.GetRandomBlock();
        if (blockToSpawn != null)
        {
            int random = Random.Range(0, 12);
            Vector2 position = new Vector2(random, transform.position.y);
            Instantiate(blockToSpawn,position, blockToSpawn.transform.rotation);
        }
    }

    void SpawnBlockFol()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        Vector2 playerPos = Player.transform.position;
        GameObject blockToSpawn = normal.GetRandomBlock();
        if (blockToSpawn != null)
        {
            int x = (int)playerPos.x;
            Vector2 position = new Vector2(x, transform.position.y);
            Instantiate(blockToSpawn,position, Quaternion.identity);
        }
    }
}

