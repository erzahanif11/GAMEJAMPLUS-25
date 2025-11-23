using System;
using UnityEngine;

public class BlockSpawn : MonoBehaviour
{


    [SerializeField] ListofBlocks normal;

    public static BlockSpawn instance;
    public float spawnInterval = 1.5f;

    public string DebugBlockName;
     private
    void Awake()
    {
        instance = this;
    }


float curtime=0;

void Start(){
    SpawnBlock();
}
    void Update()
    {
        curtime+=Time.deltaTime;
        if (curtime >= spawnInterval)
        {
            SpawnBlock();
            curtime=0;
        }
    }
GameObject blockToSpawn ;
    void SpawnBlock()
    {
        if (DebugBlockName != null)
         blockToSpawn=  normal.blockPrefabs.Find(block => block.name == DebugBlockName);
        
        if (blockToSpawn == null)
         blockToSpawn = normal.GetRandomBlock();

    
        if (blockToSpawn != null)
        {

        int x;

        if (UnityEngine.Random.value < 0.5f) //50% chance to spawn near player
          x = (int)GameObject.FindGameObjectWithTag("Player").transform.position.x ;    

            else //spawn randomly
             x = UnityEngine.Random.Range(0, 12);
            
            Vector2 position = new Vector2(x, transform.position.y);
            Instantiate(blockToSpawn,position, blockToSpawn.transform.rotation);
        }


    }


}

