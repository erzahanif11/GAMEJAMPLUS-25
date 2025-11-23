using System;
using UnityEngine;

public class BlockSpawn : MonoBehaviour
{


    [SerializeField] ListofBlocks normal;
    [SerializeField] int wallRight = 14; // right boundary for spawning
    [SerializeField] int wallLeft = -12; // left boundary for spawning

    public static BlockSpawn instance;
    public float spawnInterval = 1.5f;
    [SerializeField] GameObject player;

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
            // Parse width from block name (e.g., "BlockW3" -> width = 3)
            int width = 1; // default
            string blockName = blockToSpawn.name;
            int wIndex = blockName.LastIndexOf('W');
            if (wIndex >= 0 && wIndex < blockName.Length - 1)
            {
                if (int.TryParse(blockName.Substring(wIndex + 1), out int parsedWidth))
                    width = parsedWidth;
            }

            // Decide spawn X: 50% near player, 50% random, but clamp to bounds [0, 15-width]
            int x;
            if (UnityEngine.Random.value < 0.5f) // 50% chance to spawn near player
            {
                if (player != null)
                {
                    int playerX = (int)player.transform.position.x;
                    // Clamp player X so block doesn't go out of bounds
                    x = Mathf.Clamp(playerX, wallLeft, wallRight - width);
                }
                else
                {
                    // No player found, spawn randomly
                    x = UnityEngine.Random.Range(wallLeft, wallRight - width);
                }
            }
            else // 50% random spawn
            {
                    x = UnityEngine.Random.Range(wallLeft, wallRight - width);
            }

            Vector2 position = new Vector2(x, transform.position.y);
            Instantiate(blockToSpawn, position, blockToSpawn.transform.rotation);
        }
    }


}

