using System.Collections.Generic;
using UnityEngine;

public class BlockSpawn : MonoBehaviour
{


    [SerializeField] ListofBlocks normal;

    public static BlockSpawn instance;
    void Awake()
    {
        instance = this;
    }


float curtime=0;
    void Update()
    {
        curtime+=Time.deltaTime;
        if (curtime >= 5)
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
            int random = Random.Range(0, 13);
            Vector2 position = new Vector2(random, transform.position.y);
            Instantiate(blockToSpawn,position, Quaternion.identity);
        }
    }
}

